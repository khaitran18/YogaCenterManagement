using AutoMapper;
using Domain.Interface;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
using Ordering.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    if (@class != null) return await Task.FromResult(true);
                    else {
                        throw new BadRequestException("Invalid credential");
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

        public async Task<bool> CheckSlotInClass(int classId, int slotId)
        {
            return await Task.FromResult(_context.Schedules.Any(s => s.ClassId == classId && s.SlotId == slotId));
        }

        public async Task<string?> GetClassNotificationByClassIdAndSlotId(int classId, int slotId)
        {
            Schedule? sch = _context.Schedules.FirstOrDefault(s => (s.ClassId == classId) && (s.SlotId == slotId));
            return await Task.FromResult(sch.DailyNote);
        }
    }
}
