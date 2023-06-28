using Application.Common.Exceptions;
using AutoMapper;
using Domain.Interface;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    if (acc.Password.Equals(password))
                    {
                        //if (acc.EmailVerify == true)
                        //{
                        //    return await Task.FromResult(acc.Id);
                        //}
                        //else return await Task.FromResult(0);
                        return await Task.FromResult(acc.Uid);
                    }
                }
                return await Task.FromResult(-1);
            }
            catch (Exception)
            {
                return await Task.FromResult(-1);
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
                    Password = password,
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
            catch (Exception e)
            {
                throw e;
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
                    existingUser.Password = user.Password;
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

        public async Task<List<UserModel>> GetAll()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return _mapper.Map<List<UserModel>>(users);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserModel> DisableUser(int userId, string reason)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Uid == userId);

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
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public async Task<(int userId, string UserName, string role)> GetAccountDetailsByIdAsync(int id)
        {
            User? account = _context.Users.FirstOrDefault(a => a.Uid == id);
            UserModel acc = _mapper.Map<UserModel>(account);
            string role = _context.Roles.FirstOrDefault(p => p.RoleId == acc.RoleId).RoleName;
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
                User u = _context.Users.FirstOrDefault(u => u.VerificationToken.Equals(token));
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
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }
    }
}
