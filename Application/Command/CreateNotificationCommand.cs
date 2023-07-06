using Application.Common;
using Application.Common.Dto;
using MediatR;

namespace Application.Command
{
    public class CreateNotificationCommand : IRequest<BaseResponse<ClassNotificationDto>>
    {
        public string? token { get; set; }
        public int scheduleid { get; set; }
        public string? content { get; set; }
    }
}
