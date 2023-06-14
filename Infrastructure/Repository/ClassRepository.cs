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
    public class ClassRepository : BaseRepository<ClassModel>, IClassRepository
    {
        private readonly IMapper _mapper;
        private readonly YGCContext _context;

        public ClassRepository(YGCContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
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
