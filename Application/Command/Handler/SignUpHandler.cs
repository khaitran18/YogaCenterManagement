using Application.Common;
using Application.Common.Dto;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Service;
using AutoMapper;
using Domain.Model;
using MediatR;
using System.Security.Claims;

namespace Application.Command.Handler
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, BaseResponse<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public SignUpHandler(IUnitOfWork unitOfWork, IMailService mailService, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<BaseResponse<UserDto>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<UserDto> response = new BaseResponse<UserDto>();
            UserModel u = null;
            try
            {
                //check if username exist
                if (!await _unitOfWork.UserRepository.ExistUserName(request.UserName))
                {
                    // Check token to add role to the account
                    if (request.Token != null)
                    {
                        var role = _tokenService.ValidateToken(request.Token)?.FindFirst(ClaimTypes.Role)?.Value;
                        if (role.Equals("Staff"))
                        {
                            if ((!request.Role.Equals("Lecturer", StringComparison.OrdinalIgnoreCase)) && (!request.Role.Equals("User", StringComparison.OrdinalIgnoreCase)))
                            {
                                response.Error = true;
                                response.Exception = new UnauthorizedAccessException();
                                response.Message = "Unauthorized";
                            }
                            else u = await _unitOfWork.UserRepository.Create(request.UserName, request.Password, request.Phone, request.FullName, request.Address, request.Email, request.Role);
                        }
                        if (role.Equals("Admin"))
                        {
                            if (!request.Role.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                            {
                                response.Error = true;
                                response.Exception = new UnauthorizedAccessException();
                                response.Message = "Unauthorized";
                            }
                            else u = await _unitOfWork.UserRepository.Create(request.UserName, request.Password, request.Phone, request.FullName, request.Address, request.Email, request.Role);
                        }
                    }
                    else
                    {
                        u = await _unitOfWork.UserRepository.Create(request.UserName, request.Password, request.Phone, request.FullName, request.Address, request.Email, "User");
                    }
                    //send email verification
                    if (u != null)
                    {
                        string verifyLink = "https://localhost:7241/api/auth/verify?t=" + u.VerificationToken;
                        //Send mail
                        bool c = await _mailService.SendAsync(
    new MailDataModel
    {
        To = new List<string> { u.Email },
        Subject = "Yoga Guru",
        Body = $@"
                              <h2 style=""color: #0c7cd5; font-family: Arial, sans-serif; font-size: 24px; margin-bottom: 20px;"">Yoga Guru</h2>
  
                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;"">{request.UserName},</p>
  
                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;"">Thank you for your registration at Yoga Guru. We appreciate your support!</p>
  
                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;""> Click the following link to verify your email before proceeding: <a href='{verifyLink}'>Here</a></p>
  
                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;"">If you have any questions or need further assistance, please don't hesitate to reach out to our support team.</p>
  
                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;"">Thank you again for choosing Yoga Guru!</p>
  
                              <hr style=""border: none; border-top: 1px solid #ccc; margin: 20px 0;"">
  
                              <p style=""color: #888; font-family: Arial, sans-serif; font-size: 14px;"">Best regards,<br>The Yoga Guru Team</p>"
    }
    , new CancellationToken());
                        if (!c)
                        {
                            response.Error = true;
                            response.Exception = new BadRequestException();
                            response.Message = "Error in sending verification email";
                        }
                        // return dto
                        else response.Result = _mapper.Map<UserDto>(u);
                    }
                    else
                    {
                        response.Error = true;
                        response.Exception = new BadRequestException();
                        response.Message = "Error in creating new account";
                    }
                }
                else
                {
                    response.Error = true;
                    response.Exception = new BadRequestException();
                    response.Message = "Username exist";
                }
            }
            catch (Exception e)
            {
                response.Error = true;
                response.Exception = e;
                response.Message = e.Message;
            }
            return response;
        }
    }
}
