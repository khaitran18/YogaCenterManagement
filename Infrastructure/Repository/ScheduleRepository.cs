using AutoMapper;
using Domain.Interface;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ScheduleRepository : BaseRepository<ScheduleModel> ,IScheduleRepository
    {
        private readonly YGCContext _context;
        private readonly IMapper _mapper;

        public ScheduleRepository(YGCContext context,IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ScheduleModel> CreateNotification(int id,string notification)
        {
            try
            {
                ScheduleModel model = new ScheduleModel();
                Schedule? schedule = _context.Schedules.FirstOrDefault(s => s.Id == id);
                if (schedule != null)
                {
                    schedule.DailyNote = notification;
                    //_context.Entry(schedule).State = EntityState.Modified;
                    UpdateAsync(_mapper.Map<ScheduleModel>(schedule));
                }
                model = _mapper.Map<ScheduleModel>(schedule);
                _context.SaveChanges();
                return await Task.FromResult(model);
            }
            catch{ throw new Exception("Error in adding new daily note"); }
        }
    }
}
