using Application.Common.Dto;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mapping
{
    public class DayProfile : Profile
    {
        public DayProfile()
        {
            CreateMap<DayModel, DayDto>().ReverseMap();
        }
    }
}
