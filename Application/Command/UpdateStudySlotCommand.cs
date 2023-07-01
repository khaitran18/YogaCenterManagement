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
    public class UpdateStudySlotCommand : IRequest<BaseResponse<bool>>
    {
        public StudySlotDto StudySlot { get; set; } = null!;
    }
}
