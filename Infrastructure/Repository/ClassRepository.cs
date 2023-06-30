using Application.Common.Exceptions;
using AutoMapper;
using Domain.Interface;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
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

        public async Task<ClassModel> CreateClassSchedule(string name, double price, int capacity, DateTime startDate, DateTime endDate, List<int> dateIds)
        {
            Class newClass = new Class();
            try
            {
                newClass = new Class()
                {
                    ClassName = name,
                    Price = price,
                    ClassCapacity = capacity,
                    StartDate = startDate,
                    EndDate = endDate,
                };
                _context.Classes.Add(newClass);
                await _context.SaveChangesAsync();
                var newClassWithEmptySchedules = _context.Classes.OrderByDescending(c => c.ClassId).FirstOrDefault();
                if (newClassWithEmptySchedules == null)
                {
                    throw new Exception("Failed to retrieve the newly created class.");
                }
                //return class with empty if no day is selected 
                if (dateIds.Count() == 0)
                {
                    return _mapper.Map<ClassModel>(newClassWithEmptySchedules);
                }
                //auto generate schedules for the class
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    int selectedDay = (int)date.DayOfWeek;
                    if (dateIds.Contains(selectedDay))
                    {
                        StudySlot studySlot = _context.StudySlots.First(s => s.Days.Where(d => d.DayId == selectedDay).Any());
                        Schedule schedule = new Schedule()
                        {
                            ClassId = newClassWithEmptySchedules.ClassId,
                            SlotId = studySlot.SlotId,
                            Date = date,
                        };
                        _context.Schedules.Add(schedule);
                    }
                }
                await _context.SaveChangesAsync();
                var newClassWithSchedules = _context.Classes.Single(c => c.ClassId == newClassWithEmptySchedules.ClassId);
                return _mapper.Map<ClassModel>(newClassWithSchedules);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
            catch (Exception)
            {
                throw;
            }
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

        public async Task<(List<ClassModel>, int)> GetClasses(string? searchKeyword, string? sortBy, DateTime? startingFromDate, int? durationMonths, string? classCapacity, int page, int pageSize)
        {
            IQueryable<Class> query = _context.Classes
                .Include(c => c.Schedules)
                .Include(c => c.Lecturer);

            // Search
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(c => c.ClassName.Contains(searchKeyword));
            }

            // Starting from date filter
            if (startingFromDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= startingFromDate.Value);
            }

            // Duration filter
            if (durationMonths.HasValue)
            {
                DateTime endDate = DateTime.Now.AddMonths(durationMonths.Value);
                query = query.Where(c => c.EndDate <= endDate);
            }

            // Class capacity filter
            if (!string.IsNullOrEmpty(classCapacity))
            {
                if (classCapacity == "<15")
                {
                    query = query.Where(c => c.ClassCapacity < 15);
                }
                else if (classCapacity == "15-25")
                {
                    query = query.Where(c => c.ClassCapacity >= 15 && c.ClassCapacity <= 25);
                }
                else if (classCapacity == ">25")
                {
                    query = query.Where(c => c.ClassCapacity > 25);
                }
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "classname":
                        query = query.OrderBy(c => c.ClassName);
                        break;
                    case "classname_desc":
                        query = query.OrderByDescending(c => c.ClassName);
                        break;
                    case "price":
                        query = query.OrderBy(c => c.Price);
                        break;
                    case "price_desc":
                        query = query.OrderByDescending(c => c.Price);
                        break;
                }
            }

            int totalCount = await query.CountAsync();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            List<ClassModel> classes = await query.Select(c => _mapper.Map<ClassModel>(c)).ToListAsync();

            return (classes, totalCount);
        }


        public async Task<string?> GetClassNotificationByClassIdAndSlotId(int classId, int slotId)
        {
            try
            {
                Schedule? sch = _context.Schedules.FirstOrDefault(s => s.ClassId == classId && s.SlotId == slotId);
                return await Task.FromResult(sch?.DailyNote);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
