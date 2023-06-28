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

    }
}
