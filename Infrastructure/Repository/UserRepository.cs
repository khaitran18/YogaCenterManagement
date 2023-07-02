using Application.Common.Dto;
using Application.Common.Exceptions;
using AutoMapper;
using Domain.Interface;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository : BaseRepository<UserModel>, IUserRepository
    {
        private readonly YGCContext _context;
        private readonly IMapper _mapper;
        public UserRepository(YGCContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CheckAccountAsync(string username, string password)
        {
            try
            {
                var acc = _context.Users.FirstOrDefault(a => a.UserName.Equals(username));
                if (acc != null)
                {
                    if (PasswordHasher.Validate(acc.Password,password))
                    {
                        if (acc.IsVerified == true)
                        {
                            if (acc.IsDisabled)
                            {
                                return await Task.FromResult(-3);
                            }
                            else return await Task.FromResult(acc.Uid);
                        }
                        else return await Task.FromResult(0);     
                    }
                    else { return await Task.FromResult(-1); }
                }
                return await Task.FromResult(-2);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<UserModel> Create(string userName, string password, string phone, string fullName, string address, string email)
        {
            User user = new User();
            try
            {
                Guid uid = Guid.NewGuid();
                string uidString = uid.ToString();
                user = new User
                {
                    UserName = userName,
                    Password = PasswordHasher.Hash(password),
                    FullName = fullName,
                    Address = address,
                    Phone = phone,
                    Email = email,
                    RoleId = 1,
                    VerificationToken = uidString
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return await Task.FromResult(_mapper.Map<UserModel>(user));
        }

        public async Task<UserModel> EditProfile(UserModel user)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Uid == user.Uid);

                if (existingUser != null)
                {
                    existingUser.FullName = user.FullName;
                    existingUser.Address = user.Address;
                    existingUser.Phone = user.Phone;

                    return _mapper.Map<UserModel>(existingUser);
                }
                else
                {
                    throw new NotFoundException("User not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserModel> EditUser(UserModel user)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Uid == user.Uid);

                if (existingUser != null)
                {
                    existingUser.FullName = user.FullName;
                    existingUser.Address = user.Address;
                    existingUser.Phone = user.Phone;
                    existingUser.UserName = user.UserName;
                    existingUser.Password = PasswordHasher.Hash(user.Password);
                    existingUser.RoleId = user.RoleId;

                    return _mapper.Map<UserModel>(existingUser);
                }
                else
                {
                    throw new NotFoundException("User not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(List<UserModel>, int)> GetUsers(string? searchKeyword, int? roleId, bool? disabled, bool? verified, string? sortBy, int page, int pageSize, bool isAdmin)
        {
            IQueryable<User> query = _context.Users.Include(u => u.Role).AsQueryable();

            // Filtering
            if (!isAdmin)
            {
                query = query.Where(u => u.Role!.RoleName == "User" || u.Role.RoleName == "Lecturer");
            }
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(u => u.FullName.ToLower().Contains(searchKeyword.ToLower()));
            }
            if (roleId.HasValue)
                query = query.Where(u => u.RoleId == roleId.Value);

            if (disabled.HasValue)
                query = query.Where(u => u.IsDisabled == disabled.Value);

            if (verified.HasValue)
                query = query.Where(u => u.IsVerified == verified.Value);

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        query = query.OrderBy(u => u.FullName);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(u => u.FullName);
                        break;
                }
            }

            var totalCount = await query.CountAsync();

            // Pagination
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var users = await query.ToListAsync();

            var userModels = _mapper.Map<List<UserModel>>(users);

            return (userModels, totalCount);
        }


        public async Task<UserModel> DisableUser(int id, string reason)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Uid == id);

                if (existingUser != null)
                {
                    existingUser.IsDisabled = true;
                    existingUser.DisabledReason = reason;

                    return _mapper.Map<UserModel>(existingUser);
                }
                else
                {
                    throw new NotFoundException("User not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ExistUserName(string userName)
        {
            try
            {
                return await Task.FromResult(_context.Users.Any(u => u.UserName.Equals(userName)));
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<(int userId, string UserName, string role)> GetAccountDetailsByIdAsync(int id)
        {
            User? account = _context.Users.FirstOrDefault(a => a.Uid == id);
            UserModel acc = _mapper.Map<UserModel>(account);
            string role = _context.Roles.FirstOrDefault(p => p.RoleId == acc.RoleId)!.RoleName;
            return await Task.FromResult(
                (acc.Uid
                , acc.UserName
                , role)
                );
        }

        public async Task<bool> VerifyToken(string token)
        {
            try
            {
                User? u = _context.Users.FirstOrDefault(u => u.VerificationToken!.Equals(token));
                if (u != null)
                {
                    u.IsVerified = true;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Invalid credential");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        public async Task<int> CreateFeedback(FeedbackModel feedback)
        {
            try
            {
                var feedbackEntity = _mapper.Map<Feedback>(feedback);
                await _context.Feedbacks.AddAsync(feedbackEntity);
                await _context.SaveChangesAsync();
                return feedbackEntity.FeedbackId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IsUserLecturer(int id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Uid == id);
                if (user != null && user.Role!.RoleName == "Lecturer")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IsUserAdmin(int id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Uid == id);
                if (user != null && user.Role!.RoleName == "Admin")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(List<FeedbackModel>, int)> GetFeedbacks(int id, bool isLecturer, string? sortBy, int page, int pageSize)
        {
            IQueryable<Feedback> query = _context.Feedbacks.AsQueryable();

            if (isLecturer)
            {
                query = query
                    .Where(f => f.LecturerId == id)
                    .Select(f => new Feedback { Content = f.Content });
            }
            else
            {
                query = query.Include(f => f.Student).Include(f => f.Lecturer);
            }

            // Sort
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "time":
                        query = query.OrderBy(f => f.Time);
                        break;
                    case "time_desc":
                        query = query.OrderByDescending(f => f.Time);
                        break;
                }
            }

            var totalCount = await query.CountAsync();

            // Pagination
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var feedbacks = await query.ToListAsync();
            var feedbackModels = _mapper.Map<List<FeedbackModel>>(feedbacks);

            return (feedbackModels, totalCount);
        }
    }
}
