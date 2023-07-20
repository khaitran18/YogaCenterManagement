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
        Task<UserModel> Create(string userName, string password, string phone, string fullName, string address, string email, string role);
        Task<UserModel> EditProfile(UserModel user);
        Task<UserModel> EditUser(UserModel user);
        public Task<(List<UserModel>, int)> GetUsers(string? searchKeyword, int? roleId, bool? disabled, bool? verified, string? sortBy, int page, int pageSize, bool isStaff);
        Task<bool> VerifyToken(string token);
        Task<UserModel> DisableUser(int id, string reason);
        Task<UserModel> EnableUser(int id);
        Task<int> CreateFeedback(FeedbackModel feedback);
        Task<bool> IsUserLecturer(int id);
        Task<bool> IsUserAdmin(int id);
        Task<bool> IsUserStaff(int id);
        Task<(List<FeedbackModel>, int)> GetFeedbacks(int id, bool isLecturer, string? sortBy, int page, int pageSize);
        Task<UserModel> GetUserDetail(int userId);
        Task<string?> GetDisableReason(int userId);
        Task<int> GetUserByUsername(string userName);
    }
}