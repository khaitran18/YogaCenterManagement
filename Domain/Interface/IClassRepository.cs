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
        public Task<ClassModel> CreateClassSchedule(string name, double price, int capacity, string Description, string image, DateTime startDate, DateTime endDate, int? dateIds);
        public Task<ClassModel> AssignLecturer(int classId, int lecturerId);
        public Task<bool> CheckLecturerAuthority(int scheduleid, int userId);
        public Task<ClassModel> GetClassById(int classId);
        public Task<ClassModel> RequestChangeClass(int fromClassId, int studentId, int toClassId, string content);
        public Task<IEnumerable<ChangeClassRequestModel>> GetChangeClassRequests();
        public Task<IEnumerable<ClassModel>> GetChangeClasses(int fromClassId);
        public Task<bool> UpdateApprovalStatus(int requestId, short isApproved);
        public Task<(List<ClassModel>, int)> GetClasses(string? searchKeyword, string? sortBy, DateTime? startingFromDate, int? durationMonths, string? classCapacity, int page, int pageSize);
        public Task<PaymentModel> StudentEnrollToClass(PaymentModel paymentModel);
        public Task<ClassModel> EditClass(ClassModel model);
        public Task<(IEnumerable<ClassModel>, int)> GetStudingClass(int studentId, int page, int pageSize);
        public Task<ClassModel> GetStudyingClassByClassId(int studentId, int classId);
        public Task UpdateClassStatus();
        public Task<bool> ExistChangeClassRequest(int studentId, int fromClassId, int toClassId);
        public Task<(IEnumerable<ClassModel>, int)> GetStudiedClass(int studentId, int page, int pageSize);
        public Task<(IEnumerable<ClassModel>, int)> GetTeachingClass(int lecturerId, int page, int pageSize);
        public Task<ClassModel> GetTeachingClassByClassId(int lecturerId, int classId);

    }
}
