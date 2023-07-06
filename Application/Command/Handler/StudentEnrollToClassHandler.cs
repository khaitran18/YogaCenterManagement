using Application.Common;
using Application.Common.Dto;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Service;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handler
{
    public class StudentEnrollToClassHandler : IRequestHandler<StudentEnrollToClassCommand, BaseResponse<PaymentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenServices;
        private readonly IMailService _mailService;
        public StudentEnrollToClassHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ITokenService tokenServices, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _tokenServices = tokenServices;
            _mailService = mailService;
        }
        public async Task<BaseResponse<PaymentDto>> Handle(StudentEnrollToClassCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaymentDto>();
            try
            {
                string authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                ClaimsPrincipal claims = _tokenServices.ValidateToken(authorizationHeader);
                int studentId = int.Parse(claims.FindFirst("jti")?.Value ?? "0");
                if (studentId != request.PaymentDto.StudentId)
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("Student id not match");
                }
                else
                {
                    var @class = await _unitOfWork.ClassRepository.GetClassById(request.PaymentDto.ClassId!.Value);
                    if (@class.ClassStatus != 1)
                    {
                        response.Error = true;
                        response.Exception = new BadRequestException("Class is not able to enroll");
                    }
                    else if (@class.ClassCapacity < @class.Students?.Count)
                    {
                        response.Error = true;
                        response.Exception = new BadRequestException("Class is full");
                    }
                    else if (@class.Price != (double)request.PaymentDto.Amount)
                    {
                        response.Error = true;
                        response.Exception = new BadRequestException("Transfer amount is not match");
                    }
                    else
                    {
                        var paymentModel = _mapper.Map<PaymentModel>(request.PaymentDto);
                        var student = await _unitOfWork.UserRepository.GetUserDetail(studentId);

                        bool c = await _mailService.SendAsync(
                        new MailDataModel
                        {
                            To = new List<string> { student.Email },
                            Subject = "Yoga Guru",
                            Body = $@"
                              <h2 style=""color: #0c7cd5; font-family: Arial, sans-serif; font-size: 24px; margin-bottom: 20px;"">Yoga Guru</h2>
  
                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;"">{student.UserName},</p>
  
                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;"">Thank you for enrolling class {@class.ClassName} at Yoga Guru. We appreciate your support!</p>
  
                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;"">Your class will start on {@class.StartDate.ToString("dd/MM/yyyy")} </p>

                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;"">Your class will end on {@class.EndDate.ToString("dd/MM/yyyy")} </p>

                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;"">Your enroll fee is {@class.Price.ToString("N0")} </p>

                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;"">If you have any questions or need further assistance, please don't hesitate to reach out to our support team.</p>
  
                              <p style=""color: #555; font-family: Arial, sans-serif; font-size: 16px; margin-bottom: 10px;"">Thank you again for choosing Yoga Guru!</p>
  
                              <hr style=""border: none; border-top: 1px solid #ccc; margin: 20px 0;"">
  
                              <p style=""color: #888; font-family: Arial, sans-serif; font-size: 14px;"">Best regards,<br>The Yoga Guru Team</p>"
                        }
    , new CancellationToken());
                        if (c)
                        {
                            var result = await _unitOfWork.ClassRepository.StudentEnrollToClass(paymentModel);
                            response.Result = _mapper.Map<PaymentDto>(result);
                        }
                        else
                        {
                            response.Error = true;
                            response.Exception = new BadRequestException();
                            response.Message = "Error when enroll class";
                        }
                    }
                }

            }
            catch (Exception)
            {
                response.Error = true;
                throw;
            }
            return response;
        }
    }
}
