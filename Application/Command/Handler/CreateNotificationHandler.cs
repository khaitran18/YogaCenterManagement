using Application.Common.Dto;
using Application.Interfaces;
using Application.Service;
using MediatR;
using Application.Common.Exceptions;
using System.Security.Claims;
using Application.Common;

namespace Application.Command.Handler
{
    public class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand, BaseResponse<ClassNotificationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;

        public CreateNotificationHandler(IUnitOfWork unitOfWork, ITokenService tokenServices)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
        }

        public async Task<BaseResponse<ClassNotificationDto>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<ClassNotificationDto> response = new BaseResponse<ClassNotificationDto>();
            try
            {
                ClaimsPrincipal claims = _tokenServices.ValidateToken(request.token);
                if (claims != null)
                {
                    int.TryParse(claims.FindFirst("jti")?.Value, out int userId);
                    if (await _unitOfWork.ClassRepository.CheckLecturerAuthority(request.scheduleid, userId))
                    {
                         await _unitOfWork.ScheduleRepository.CreateNotification(request.scheduleid, request.content);
                    }
                    else
                    {
                        response.Error = true;
                        response.Exception = new BadRequestException("This account have no authority to change this information");
                    }
                }
                else
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("Invalid credential");
                }
                response.Result = new ClassNotificationDto { content = request.content };
                return response;
            }
            catch (Exception e)
            {
                response.Error = true;
                response.Exception = e;
                return response;
            }
        }
    }
}
