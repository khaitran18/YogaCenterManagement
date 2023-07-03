using AutoMapper;
using Domain.Model;
using Infrastructure.DataModels;

namespace Infrastructure.Service.Mapper
{
    public class StudySlotMapper : Profile
    {
        public StudySlotMapper()
        {
            CreateMap<StudySlot, StudySlotModel>()
                .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Days.Select(d => new DayModel { DayId = d.DayId, Day = d.Day }).ToList()))
                .ReverseMap()
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.Day.Select(dm => new DateOfWeek { DayId = dm.DayId, Day = dm.Day }).ToList()));

        }
    }
}
