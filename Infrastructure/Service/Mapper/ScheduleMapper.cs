using AutoMapper;
using Domain.Model;
using Infrastructure.DataModels;

namespace Infrastructure.Service.Mapper
{
    public class ScheduleMapper : Profile
    {
        public ScheduleMapper()
        {
            CreateMap<Schedule, ScheduleModel>().ReverseMap();
        }
    }
}
