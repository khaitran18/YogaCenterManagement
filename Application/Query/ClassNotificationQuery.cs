using Application.Common;
using Application.Common.Dto;
using MediatR;
namespace Application.Query
{
    public class ClassNotificationQuery : IRequest<BaseResponse<ClassNotificationDto>>
    {
        public int scheduleId { get; set; }
    }
}
