using AutoMapper;
using Domain.Model;
using Infrastructure.DataModels;

namespace Infrastructure.Service.Mapper
{
    public class ClassMapper : Profile
    {
        public ClassMapper()
        {
            CreateMap<Class, ClassModel>().ReverseMap();
        }
    }
}
