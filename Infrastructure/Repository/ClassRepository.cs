using AutoMapper;
using Domain.Interface;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
using Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ClassRepository : BaseRepository<ClassModel>, IClassRepository
    {
        private readonly IMapper _mapper;
        private readonly YGCContext _context;

        public ClassRepository(YGCContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckLecturerAuthority(int scheduleid, int userId)
        {
            try
            {
                Schedule? schedule = _context.Schedules.FirstOrDefault(s => s.Id == scheduleid);
                if (schedule != null)
                {
                    Class? @class = _context.Classes.FirstOrDefault(c => (c.ClassId == schedule.ClassId) && (c.LecturerId == userId));
                    if (@class != null)
                        return await Task.FromResult(true);
                    else
                    {
                        return await Task.FromResult(false);
                    }
                }
                else throw new NotFoundException("Schedule not found");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> CheckSlotInClass(int classId, int slotId)
        {
            return await Task.FromResult(_context.Schedules.Any(s => s.ClassId == classId && s.SlotId == slotId));
        }

        public async Task<ClassModel> GetClassById(int classId)
        {
            try
            {
                var entityClass = await _context.Classes
                    .Include(c => c.Schedules)
                        .ThenInclude(s => s.Slot)
                    .Include(s => s.Lecturer)
                    .Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.ClassId == classId);

                if (entityClass == null)
                {
                    throw new NotFoundException("Class not found");
                }

                ClassModel classModel = _mapper.Map<ClassModel>(entityClass);
                return classModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> GetClassNotificationByClassIdAndSlotId(int classId, int slotId)
        {
            Schedule? sch = _context.Schedules.FirstOrDefault(s => (s.ClassId == classId) && (s.SlotId == slotId));
            return await Task.FromResult(sch.DailyNote);
        }

        #region Create a student change class request
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromClassId">Id of the student current class</param>
        /// <param name="studentId">Student Id make request</param>
        /// <param name="toClassId">Id of the class that student want to change</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ClassModel> RequestChangeClass(int fromClassId, int studentId, int toClassId, string content)
        {
            var classModel = new ClassModel();
            try
            {
                var fromClass = await _context.Classes.Include(fc => fc.Students).FirstOrDefaultAsync(fc => fc.ClassId == fromClassId);
                var isMatchSchedule = await IsMatchSchedule(fromClassId, toClassId);
                if (isMatchSchedule)
                {
                    if (fromClass != null)
                    {
                        if (fromClass.Students.FirstOrDefault(st => st.Uid == studentId) != null)
                        {
                            _context.ChangeClassRequests.Add(new ChangeClassRequest
                            {
                                UserId = studentId,
                                ClassId = fromClassId,
                                RequestClassId = toClassId,
                                Content = content
                            });
                            await _context.SaveChangesAsync();

                            var requestClass = await _context.Classes.FindAsync(toClassId);

                            classModel = _mapper.Map<ClassModel>(requestClass);
                        }

                    }
                }

            }
            catch (Exception)
            {

                throw new Exception("Error when create request change class");
            }
            return classModel;
        }
        #endregion

        #region Get all class change requests
        /// <summary>
        /// Get all class change requests
        /// </summary>
        /// <returns>List of change class requests</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<ChangeClassRequestModel>> GetChangeClassRequests()
        {
            var classRequestModels = new List<ChangeClassRequestModel>();
            try
            {
                var classRequests = await _context.ChangeClassRequests.ToListAsync();
                classRequestModels = _mapper.Map<List<ChangeClassRequestModel>>(classRequests);
            }
            catch (Exception)
            {

                throw new Exception("Error in get all class change requests");
            }
            return classRequestModels;
        }
        #endregion

        #region Update approval status of a change class request
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId">Id of the request</param>
        /// <param name="isApproved">Approve status for the request</param>
        /// <returns>True if success; False if unsuccess</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> UpdateApprovalStatus(int requestId, bool isApproved)
        {
            try
            {
                var changeRequest = await _context.ChangeClassRequests.FindAsync(requestId);

                if (changeRequest != null)
                {
                    changeRequest.IsApproved = isApproved;
                    await _context.SaveChangesAsync();
                    if (isApproved)
                    {
                        var classFound = await _context.Classes.FindAsync(changeRequest.RequestClassId);

                        if (classFound != null)
                        {
                            var student = await _context.Users.FirstOrDefaultAsync(st => st.Uid == changeRequest.UserId);

                            if (student != null)
                            {
                                var originalClass = await _context.Classes.Include(c => c.Students).FirstOrDefaultAsync(oc => oc.ClassId == changeRequest.ClassId);
                                 
                                if (originalClass != null)
                                {
                                    originalClass.Students.Remove(student);
                                    classFound.Students.Add(student);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                throw new Exception("Error when updating approval status");
            }
        }
        #endregion

        #region Check similarity of 2 class schedule
        private async Task<bool> IsMatchSchedule(int fromClassId, int toClassId)
        {
            try
            {
                var fromClass = await _context.Classes.Include(fc => fc.Schedules).FirstOrDefaultAsync(fc => fc.ClassId == fromClassId);
                var toClass = await _context.Classes.Include(tc => tc.Schedules).FirstOrDefaultAsync(tc => tc.ClassId == toClassId);
                if(fromClass != null && toClass != null)
                {
                    if(fromClass.StartDate == toClass.StartDate && fromClass.EndDate == toClass.EndDate)
                    {
                        if(fromClass.Schedules.Count == toClass.Schedules.Count)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw new Exception("Error in IsMatchSchedule");
            }
            return false;
        }
        #endregion
    }
}
