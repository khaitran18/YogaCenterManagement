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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handler
{
    public class DisableUserHandler : IRequestHandler<DisableUserCommand, BaseResponse<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DisableUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<UserDto>> Handle(DisableUserCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<UserDto> response = new BaseResponse<UserDto>();

            try
            {
                var disabledUser = await _unitOfWork.UserRepository.DisableUser(request.UserId, request.Reason);
                await _unitOfWork.Save();

                var userDto = _mapper.Map<UserDto>(disabledUser);

                response.Result = userDto;
                response.Message = "User disabled successfully!";
            }
            catch (NotFoundException ex)
            {
                response.Error = true;
                response.Message = ex.Message;
                response.Exception = ex;
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Message = ex.Message;
                response.Exception = ex;
            }

            return response;
        }
    }
}
