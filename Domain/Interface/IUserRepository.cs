using Application.Interfaces;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IUserRepository:IBaseRepository<UserModel>
    {
        Task<int> CheckAccountAsync(string username, string password);
        Task<(int userId, string UserName, string role)> GetAccountDetailsByIdAsync(int id);
        Task<bool> ExistUserName(string userName);
        Task<UserModel> Create(string userName, string password, string phone, string fullName, string address, string email);
        Task<UserModel> EditProfile(UserModel user);
        Task<UserModel> EditUser(UserModel user);
        Task<List<UserModel>> GetAll();
        Task<UserModel> DisableUser(int userId, string reason);
        Task<bool> VerifyToken(string token);
    }
}