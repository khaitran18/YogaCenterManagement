using Application.Common.Dto;
using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;
using Application.Common;

namespace Application.Query.Handler
{
    public class ClassNotificationHandler : IRequestHandler<ClassNotificationQuery, BaseResponse<ClassNotificationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClassNotificationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<ClassNotificationDto>> Handle(ClassNotificationQuery request, CancellationToken cancellationToken)
        {
            BaseResponse<ClassNotificationDto> response = new BaseResponse<ClassNotificationDto>();
            try
            {
                if (await _unitOfWork.ClassRepository.CheckSlotInClass(request.classId, request.slotId))
                {
                    string content = await _unitOfWork.ClassRepository.GetClassNotificationByClassIdAndSlotId(request.classId, request.slotId);
                    response.Result = new ClassNotificationDto
                    {
                        content = content,
                    };
                }
                else
                {
                    response.Error = true;
                    response.Exception = new NotFoundException("Schedule for class is not found");
                }
                return response;
            }
            catch(Exception e)
            {
                response.Error = true;
                response.Exception = e;
                response.Message = e.Message;
                return response;
            }
        }
    }
}
