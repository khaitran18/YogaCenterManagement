using AutoMapper;
using Domain.Model;
using Infrastructure.DataModels;

namespace Infrastructure.Service.Mapper
{
    public class AvailableDateMapper : Profile
    {
        public AvailableDateMapper()
        {
            CreateMap<AvailableDate, AvailableDateModel>().ReverseMap();
        }
    }
}
