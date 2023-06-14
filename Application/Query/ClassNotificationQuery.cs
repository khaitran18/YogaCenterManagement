using Application.Common.Dto;
using MediatR;
namespace Application.Query
{
    public class ClassNotificationQuery : IRequest<ClassNotificationDto>
    {
        public int classId { get; set; }
        public int slotId { get; set; }
    }
}
