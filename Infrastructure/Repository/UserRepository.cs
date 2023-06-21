using AutoMapper;
using Domain.Interface;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
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
    }
}
