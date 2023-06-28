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
    public class CreateFeedbackCommand : IRequest<BaseResponse<FeedbackDto>>
    {
        public string? Token { get; set; }
        public string Content { get; set; } = null!;
        public int LecturerId { get; set; }
    }
}
