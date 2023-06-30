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
    public class EditUserHandler : IRequestHandler<EditUserCommand, BaseResponse<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<UserDto>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<UserDto> response = new BaseResponse<UserDto>();

            try
            {
                var userModel = new UserModel
                {
                    Uid = request.Uid,
                    UserName = request.UserName,
                    Password = request.Password,
                    FullName = request.FullName,
                    Address = request.Address,
                    Phone = request.Phone,
                    RoleId = request.RoleId
                };

                var editedUser = await _unitOfWork.UserRepository.EditUser(userModel);
                await _unitOfWork.Save();

                var userDto = _mapper.Map<UserDto>(editedUser);

                response.Result = userDto;
                response.Message = "User information updated successfully!";
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
