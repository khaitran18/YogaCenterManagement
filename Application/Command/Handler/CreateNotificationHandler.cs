using Application.Common.Dto;
using Application.Interfaces;
using Application.Service;
using MediatR;
using Ordering.Application.Common.Exceptions;
using System.Security.Claims;

namespace Application.Command.Handler
{
    public class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand,Task>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;

        public CreateNotificationHandler(IUnitOfWork unitOfWork, ITokenService tokenServices)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
        }

        public async Task<Task> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ClaimsPrincipal claims = _tokenServices.ValidateToken(request.token);
                if (claims != null)
                {
                    int.TryParse(claims.FindFirst("jti")?.Value,out int userId);
                    if (await _unitOfWork.ClassRepository.CheckLecturerAuthority(request.scheduleid, userId))
                    {
                        await _unitOfWork.ScheduleRepository.CreateNotification(request.scheduleid, request.content);
                    }
                    else
                    {
                        throw new BadRequestException("Account cant change this information");
                    }
                }
                else
                {
                    throw new BadRequestException("Invalid credential");
                }
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
