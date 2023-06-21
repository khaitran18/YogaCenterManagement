using Application.Common.Dto;
using Application.Interfaces;
using Application.Service;
using MediatR;
using Application.Common.Exceptions;
using Application.Common;

namespace Application.Command.Handler
{
    public class AuthHandler : IRequestHandler<AuthCommand, BaseResponse<AuthResponseDto>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthHandler(ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<AuthResponseDto>> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<AuthResponseDto> authResponse = new BaseResponse<AuthResponseDto>();
            try
            {
                int userId = await _unitOfWork.UserRepository.CheckAccountAsync(request.UserName, request.Password);
                if (userId == -1)
                {
                    authResponse.Error = true;
                    authResponse.Exception = new BadRequestException("Wrong username or password");
                }
                else if (userId == 0)
                {
                    authResponse.Error = true;
                    authResponse.Exception = new BadRequestException("Email not verify");
                }
                else
                {
                    var (id, username, plan) = await _unitOfWork.UserRepository.GetAccountDetailsByIdAsync(userId);
                    string token = _tokenService.GenerateJWTToken((userId: id, userName: username, plan: plan));
                    authResponse.Result =  new AuthResponseDto()
                    {
                        UserId = userId,
                        Name = username,
                        Role = plan,
                        Token = token
                    };
                }
                return authResponse;
            }
            catch (Exception e)
            {
                authResponse.Error = true;
                authResponse.Exception = e;
                return authResponse;
            }
        }
    }
}
