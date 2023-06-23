using Application.Common;
using Application.Common.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class CreateStudySlotCommand : IRequest<BaseResponse<StudySlotDto>>
    {
        public string? token { get; set; }
        public TimeSpan startTime { get; set; }
            
        public TimeSpan endTime { get; set; }
        public List<int> dateIds { get; set; } = new List<int>();
    }
}
