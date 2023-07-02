using Application.Interfaces;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IClassRepository : IBaseRepository<ClassModel>
    {
        public Task<ClassModel> CreateClassSchedule(string name, double price, int capacity, DateTime startDate, DateTime endDate, List<int> dateIds);
        public Task<ClassModel> AssignLecturer(int classId, int lecturerId);
        public Task<bool> CheckLecturerAuthority(int scheduleid, int userId);
        public Task<ClassModel> GetClassById(int classId);
        public Task<ClassModel> RequestChangeClass(int fromClassId, int studentId, int toClassId, string content);
        public Task<IEnumerable<ChangeClassRequestModel>> GetChangeClassRequests();
        public Task<IEnumerable<ClassModel>> GetChangeClasses(int fromClassId);
        public Task<bool> UpdateApprovalStatus(int requestId, short isApproved);
        public Task<(List<ClassModel>, int)> GetClasses(string? searchKeyword, string? sortBy, DateTime? startingFromDate, int? durationMonths, string? classCapacity, int page, int pageSize);
        public Task<PaymentModel> StudentEnrollToClass(PaymentModel paymentModel);
    }
}
