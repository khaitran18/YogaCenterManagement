using Application.Dto;
using Application.Interfaces;
using Application.Service;
using MediatR;
using Ordering.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handler
{
    public class AuthHandler : IRequestHandler<AuthCommand, AuthResponseDto>
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthHandler(ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }
        public async Task<AuthResponseDto> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            int userId = await _unitOfWork.UserRepository.CheckAccountAsync(request.UserName, request.Password);
            if (userId == -1)
            {
                throw new BadRequestException("Wrong username or password");
            }
            if (userId == 0)
            {
                throw new BadRequestException("Email not verify");
            }
            else
            {
                var (id, username, plan) = await _unitOfWork.UserRepository.GetAccountDetailsByIdAsync(userId);
                string token = _tokenService.GenerateJWTToken((userId: id, userName: username, plan: plan));
                return new AuthResponseDto()
                {
                    UserId = userId,
                    Name = username,
                    Role = plan,
                    Token = token
                };
            }
        }
    }
}
