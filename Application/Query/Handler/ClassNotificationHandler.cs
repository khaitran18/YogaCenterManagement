using Application.Common.Dto;
using Application.Interfaces;
using MediatR;
using Ordering.Application.Common.Exceptions;

namespace Application.Query.Handler
{
    public class ClassNotificationHandler : IRequestHandler<ClassNotificationQuery, ClassNotificationDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClassNotificationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClassNotificationDto> Handle(ClassNotificationQuery request, CancellationToken cancellationToken)
        {
                ClassNotificationDto dto = new ClassNotificationDto();
                if (await _unitOfWork.ClassRepository.CheckSlotInClass(request.classId, request.slotId))
                {
                    dto.content = await _unitOfWork.ClassRepository.GetClassNotificationByClassIdAndSlotId(request.classId, request.slotId);
                    return dto;
                }
                else throw new NotFoundException("Schedule for class is not found");  
        }
    }
}
