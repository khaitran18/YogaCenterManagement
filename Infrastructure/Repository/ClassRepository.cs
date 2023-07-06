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


        public async Task<ClassModel> CreateClassSchedule(string name, double price, int capacity,string description,string image, DateTime startDate, DateTime endDate, int? slotId)

        {
            Class newClass = new Class();
            try
            {
                newClass = new Class()
                {
                    ClassName = name,
                    Price = price,
                    ClassCapacity = capacity,
                    Description = description,
                    Image = image,
                    StartDate = startDate,
                    EndDate = endDate,
                    ClassStatus = 1, //default value
                };
                _context.Classes.Add(newClass);
                await _context.SaveChangesAsync();
                var newClassWithEmptySchedules = _context.Classes.OrderByDescending(c => c.ClassId).FirstOrDefault();
                if (newClassWithEmptySchedules == null)
                {
                    throw new Exception("Failed to retrieve the newly created class.");
                }
                //return class with empty if no day is selected 
                if (slotId == null || startDate == null)
                {
                    return _mapper.Map<ClassModel>(newClassWithEmptySchedules);
                }
                // validate slot
                //StudySlot slot = _context.StudySlots.Find(slotId);
                StudySlot slot = await _context.StudySlots.Include(ss => ss.Days).FirstOrDefaultAsync(ss => ss.SlotId == slotId);
                if (slot == null || slot.Days == null)
                {
                    return _mapper.Map<ClassModel>(newClassWithEmptySchedules);
                }
                //auto generate schedules for the class
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    int selectedDay = (int)date.DayOfWeek;
                    if(selectedDay == 0)
                    {
                        selectedDay = 7;
                    }
                    if (slot.Days.Where(d => d.DayId == selectedDay).Any())
                    {
                        Schedule schedule = new Schedule()
                        {
                            ClassId = newClassWithEmptySchedules.ClassId,
                            SlotId = slot.SlotId,
                            Date = date,
                        };
                        _context.Schedules.Add(schedule);
                    }
                    else
                    {
                        Console.WriteLine("Date not matched");
                    }
                    //if (dateIds.Contains(selectedDay))
                    //{
                    //    StudySlot studySlot = _context.StudySlots.First(s => s.Days.Where(d => d.DayId == selectedDay).Any());
                    //    Schedule schedule = new Schedule()
                    //    {
                    //        ClassId = newClassWithEmptySchedules.ClassId,
                    //        SlotId = studySlot.SlotId,
                    //        Date = date,
                    //    };
                    //    _context.Schedules.Add(schedule);
                    //}
                }
                await _context.SaveChangesAsync();
                var newClassWithSchedules = _context.Classes.Single(c => c.ClassId == newClassWithEmptySchedules.ClassId);
                var entityClass = await _context.Classes
                    .Include(c => c.Schedules)
                        .ThenInclude(s => s.Slot)
                    .Include(s => s.Lecturer)
                    .Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.ClassId == newClassWithEmptySchedules.ClassId);
                return _mapper.Map<ClassModel>(entityClass);
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

        public async Task<ClassModel> AssignLecturer(int classId, int lecId)
        {
            Class result = new Class();
            try
            {
                //check class exist
                var theClass = _context.Classes.Find(classId);
                if (theClass == null)
                {
                    throw new Exception("Class not found");
                }
                //set null to lecturer
                if(lecId == 0)
                {
                    theClass.LecturerId = null;
                    _context.Classes.Update(theClass);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<ClassModel>(theClass);
                }
                //check lecturer exist
                var lecturer = _context.Users.Single(u => u.Uid == lecId && u.RoleId == 2);
                if (lecturer == null)
                {
                    throw new Exception("Lecturer not found");
                }
                //get assigning schedule from the classId
                var assigningSchedule = await _context.Schedules.FirstAsync(s => s.ClassId == classId);
                //get available date of the assigning lecturer
                var availableDate = _context.AvailableDates.Where(ad => ad.LecturerId == lecId).ToList();
                //check if lecturer is free
                var currentTeachingClass = await _context.Classes.Include(c => c.Schedules).FirstOrDefaultAsync(c => c.ClassStatus != 3 && c.LecturerId == lecId);
                if(currentTeachingClass != null && currentTeachingClass.Schedules.First().SlotId == assigningSchedule.SlotId)
                {
                    throw new Exception("This slot is currently assigned to the lecturer.");
                }
                //check availableDate then assign the lecturer to the class
                if(availableDate.Where(ad => ad.SlotId == assigningSchedule.SlotId).Any())
                {
                    theClass.LecturerId = lecturer.Uid;
                    _context.Classes.Update(theClass);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<ClassModel>(theClass);
                }
                else
                {
                    throw new Exception("Lecturer is not available for the class schedules.");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<ClassModel> EditClass(ClassModel model)
        {
            Class result = new Class();
            try
            {
                var existingClass = await _context.Classes.FirstOrDefaultAsync(c => c.ClassId == model.ClassId);
                if (existingClass != null)
                {
                    existingClass.ClassName = model.ClassName;
                    existingClass.Price = model.Price;
                    existingClass.ClassCapacity = model.ClassCapacity;
                    existingClass.Description = model.Description;
                    if(model.Image != null && model.Image != "")
                    {
                        existingClass.Image = model.Image;
                    }
                    _context.Classes.Update(existingClass);
                    await _context.SaveChangesAsync();
                    var updatedClass = await _context.Classes.FirstOrDefaultAsync(c => c.ClassId == existingClass.ClassId);
                    return _mapper.Map<ClassModel>(updatedClass);
                }
                else
                {
                    throw new NotFoundException("Class not found");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<ClassModel> GetClassById(int classId)
        {
            try
            {
                var entityClass = await _context.Classes
                    .Include(c => c.Schedules)
                        .ThenInclude(s => s.Slot)
                            .ThenInclude(sl => sl!.Days)
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
                query = query.Where(c => c.ClassName.ToLower().Contains(searchKeyword.ToLower()));
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
                if (classCapacity == "lt15")
                {
                    query = query.Where(c => c.ClassCapacity < 15);
                }
                else if (classCapacity == "15-25")
                {
                    query = query.Where(c => c.ClassCapacity >= 15 && c.ClassCapacity <= 25);
                }
                else if (classCapacity == "gt25")
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
                                Content = content,
                                IsApproved = -1
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
                var classRequests = await _context.ChangeClassRequests.Include(cr => cr.Class).Include(cr => cr.RequestClass).Include(cr => cr.User).ToListAsync();
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
        public async Task<bool> UpdateApprovalStatus(int requestId, short isApproved)
        {
            try
            {
                var changeRequest = await _context.ChangeClassRequests.FindAsync(requestId);

                if (changeRequest != null)
                {
                    changeRequest.IsApproved = isApproved;
                    await _context.SaveChangesAsync();
                    if (isApproved == 1)
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
                if (fromClass != null && toClass != null)
                {
                    if (fromClass.StartDate == toClass.StartDate && fromClass.EndDate == toClass.EndDate)
                    {
                        if (fromClass.Schedules.Count == toClass.Schedules.Count)
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

        #region Enroll student to a class
        public async Task<PaymentModel> StudentEnrollToClass(PaymentModel paymentModel)
        {
            try
            {
                var payment = _mapper.Map<Payment>(paymentModel);
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                var classId = paymentModel.ClassId;
                var studentId = paymentModel.StudentId;
                if (classId != null && studentId != null)
                {
                    var @class = await _context.Classes.FindAsync(classId);
                    var student = await _context.Users.FindAsync(studentId);

                    if (@class != null && student != null)
                    {
                        @class.Students.Add(student);
                        await _context.SaveChangesAsync();
                    }
                }

                // Map the created payment back to the PaymentModel
                var createdPaymentModel = _mapper.Map<PaymentModel>(payment);

                return createdPaymentModel;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Change Classes
        public async Task<IEnumerable<ClassModel>> GetChangeClasses(int fromClassId)
        {
            var classModels = new List<ClassModel>();
            try
            {
                var classes = new List<Class>();
                var allClasses = await _context.Classes.ToListAsync();
                foreach (var @class in allClasses)
                {
                    if (await IsMatchSchedule(fromClassId, @class.ClassId) && @class.ClassId != fromClassId)
                    {
                        classes.Add(@class);
                    }
                }
                classModels = _mapper.Map<List<ClassModel>>(classes);
            }
            catch (Exception)
            {

                throw new Exception("Error in Get Change Classes");
            }
            return classModels;
        }
        #endregion

        #region Get studying class by student id
        public async Task<(IEnumerable<ClassModel>, int)> GetStudingClass(int studentId, int page, int pageSize)
        {
            var classModels = new List<ClassModel>();
            int totalCount = 0;
            try
            {
                var classes = await _context.Classes
                                                    .Include(c => c.Students)
                                                    .Where(c => c.ClassStatus == 2 && c.Students.Any(s => s.Uid == studentId))
                                                    .ToListAsync();
                classModels = _mapper.Map<List<ClassModel>>(classes.Skip((page - 1) * pageSize).Take(pageSize));
                totalCount = classes.Count;
            }
            catch (Exception)
            {

                throw;
            }
            return (classModels, totalCount);
        }
        #endregion

        #region Get specific studying class with class id
        public async Task<ClassModel> GetStudyingClassByClassId(int studentId, int classId)
        {
            var classModel = new ClassModel();
            try
            {
                var @class = await _context.Classes
                    .Include(c => c.Schedules)
                        .ThenInclude(sc => sc.Slot)
                            .ThenInclude(sl => sl.Days)
                    .Include(c => c.Lecturer)
                    .FirstOrDefaultAsync(c => c.ClassStatus == 2
                                                                 && c.ClassId == classId
                                                                 && c.Students.Any(s => s.Uid == studentId));
                classModel = _mapper.Map<ClassModel>(@class);
            }
            catch (Exception)
            {

                throw;
            }
            return classModel;
        }
        #endregion

        #region Update class status with today time
        public async Task UpdateClassStatus()
        {
            DateTime today = DateTime.Today;
            var classes = await _context.Classes.ToListAsync();

            foreach (var @class in classes)
            {
                if ((today >= @class.StartDate && today <= @class.EndDate) && @class.ClassStatus != 0)
                {
                    @class.ClassStatus = 2;
                }
                else if (today > @class.EndDate && @class.ClassStatus != 0)
                {
                    @class.ClassStatus = 3;
                }
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Check if change class request exist
        public async Task<bool> ExistChangeClassRequest(int studentId, int fromClassId, int toClassId)
        {
            try
            {
                var existingRequest = await _context.ChangeClassRequests
                    .FirstOrDefaultAsync(c =>
                        c.UserId == studentId &&
                        c.ClassId == fromClassId &&
                        c.RequestClassId == toClassId);

                return existingRequest != null;
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately (e.g., logging, error handling)
                throw new Exception("Error in ExistChangeClassRequest", ex);
            }
        }

        #endregion

        #region Get class that student has studied
        public async Task<(IEnumerable<ClassModel>, int)> GetStudiedClass(int studentId, int page, int pageSize)
        {

            var classModels = new List<ClassModel>();
            int totalCount = 0;
            try
            {
                var classes = await _context.Classes
                                                    .Include(c => c.Students)
                                                    .Where(c => c.ClassStatus == 3 && c.Students.Any(s => s.Uid == studentId))
                                                    .ToListAsync();
                classModels = _mapper.Map<List<ClassModel>>(classes.Skip((page - 1) * pageSize).Take(pageSize));
                totalCount = classes.Count;
            }
            catch (Exception)
            {

                throw new Exception("Error in GetStudiedClass");
            }
            return (classModels, totalCount);
        }
        #endregion

        #region Get teaching class with lecturer id 
        public async Task<(IEnumerable<ClassModel>, int)> GetTeachingClass(int lecturerId, int page, int pageSize)
        {
            var classModels = new List<ClassModel>();
            int totalCount = 0;
            try
            {
                var classes = await _context.Classes
                                                    .Where(c => c.ClassStatus == 2 && c.LecturerId == lecturerId)
                                                    .ToListAsync();
                classModels = _mapper.Map<List<ClassModel>>(classes.Skip((page - 1) * pageSize).Take(pageSize));
                totalCount = classes.Count;
            }
            catch (Exception)
            {

                throw;
            }
            return (classModels, totalCount);
        }
        #endregion

        #region Get teaching class with class id
        public async Task<ClassModel> GetTeachingClassByClassId(int lecturerId, int classId)
        {
            var classModel = new ClassModel();
            try
            {
                var @class = await _context.Classes
                    .Include(c => c.Schedules)
                        .ThenInclude(sc => sc.Slot)
                            .ThenInclude(sl => sl.Days)
                    .Include(c => c.Lecturer)
                    .Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.ClassStatus == 2
                                                                 && c.ClassId == classId
                                                                 && c.LecturerId == lecturerId);
                classModel = _mapper.Map<ClassModel>(@class);
            }
            catch (Exception)
            {

                throw;
            }
            return classModel;
        }
        #endregion

        #region Get classes that lecturer has taught
        public async Task<(IEnumerable<ClassModel>, int)> GetTaughtClass(int lecturerId, int page, int pageSize)
        {
            var classModels = new List<ClassModel>();
            int totalCount = 0;
            try
            {
                var classes = await _context.Classes
                                                    .Include(c => c.Students)
                                                    .Where(c => c.ClassStatus == 3 && c.Students.Any(s => s.Uid == lecturerId))
                                                    .ToListAsync();
                classModels = _mapper.Map<List<ClassModel>>(classes.Skip((page - 1) * pageSize).Take(pageSize));
                totalCount = classes.Count;
            }
            catch (Exception)
            {

                throw new Exception("Error in GetStudiedClass");
            }
            return (classModels, totalCount);
        }
        #endregion
    }
}
