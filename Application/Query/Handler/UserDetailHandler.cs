using Application.Common;
using Application.Common.Dto;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Service;
using AutoMapper;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.Handler
{
    public class UserDetailHandler : IRequestHandler<UserDetailQuery, BaseResponse<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserDetailHandler(IUnitOfWork unitOfWork, IMapper mapper,ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<BaseResponse<UserDto>> Handle(UserDetailQuery request, CancellationToken cancellationToken)
        {
            BaseResponse<UserDto> response = new BaseResponse<UserDto>();
            try
            {
                if (request.Token == null)
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("No credential found please login");
                    response.Message = "No credential found please login";
                }
                else
                {
                    string? id = _tokenService.ValidateToken(request.Token)?.FindFirst("jti")?.Value;
                    int.TryParse(id, out int userId);
                    UserModel user = await _unitOfWork.UserRepository.GetUserDetail(userId);
                    response.Result = _mapper.Map<UserDto>(user);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }
}
