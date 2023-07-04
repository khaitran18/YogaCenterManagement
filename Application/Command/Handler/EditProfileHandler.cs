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
    public class EditProfileHandler : IRequestHandler<EditProfileCommand, BaseResponse<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenServices;

        public EditProfileHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenServices = tokenServices;
        }

        public async Task<BaseResponse<UserDto>> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<UserDto> response = new BaseResponse<UserDto>();

            try
            {
                ClaimsPrincipal claims = _tokenServices.ValidateToken(request.Token ?? "");
                if (claims != null)
                {
                    int.TryParse(claims.FindFirst("jti")?.Value, out int userId);

                    var userModel = new UserModel
                    {
                        Uid = userId,
                        FullName = request.FullName,
                        Address = request.Address,
                        Phone = request.Phone,
                        Email = request.Email,
                    };

                    var editedUser = await _unitOfWork.UserRepository.EditProfile(userModel);
                    await _unitOfWork.Save();

                    var userDto = _mapper.Map<UserDto>(editedUser);

                    response.Result = userDto;
                    response.Message = "User profile updated successfully!";
                }
                else
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("Invalid credential");
                }
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
