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
                //return class with empty if no day is selected 
                if(dateIds.Count() == 0)
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
            catch (Exception e)
            {
                throw e;
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
                    else {
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
    }
}
