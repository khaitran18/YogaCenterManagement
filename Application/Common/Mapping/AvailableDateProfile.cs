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
    public class AvailableDateProfile : Profile
    {
        public AvailableDateProfile()
        {
            CreateMap<AvailableDateDto, AvailableDateModel>();
            CreateMap<AvailableDateModel, AvailableDateDto>();
        }
    }
}
