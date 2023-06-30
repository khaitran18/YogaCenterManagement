using AutoMapper;
using Domain.Model;
using Infrastructure.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service.Mapper
{
    public class ChangeClassRequestMapper : Profile
    {
        public ChangeClassRequestMapper()
        {
            CreateMap<ChangeClassRequestModel, ChangeClassRequest>();
            CreateMap<ChangeClassRequest, ChangeClassRequestModel>();
        }
    }
}
