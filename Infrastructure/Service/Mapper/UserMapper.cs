using AutoMapper;
using Domain.Model;
using Infrastructure.DataModels;

namespace Infrastructure.Service.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
