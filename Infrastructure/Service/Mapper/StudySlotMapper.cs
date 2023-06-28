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
                    .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Days.Select(d => d.Day).ToList()));
        }
    }
}
