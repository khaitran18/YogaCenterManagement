using Application.Common;
using Application.Common.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query
{
    public record GetScheduleQuery : IRequest<BaseResponse<List<ScheduleDto>>>
    {
        public string? token { get; set; }
        public int classId { get; set; }
        public DateTime startDate { get; set; }   
        public DateTime endDate { get; set; }
    }
}
