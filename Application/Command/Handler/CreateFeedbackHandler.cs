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
    public class CreateFeedbackHandler : IRequestHandler<CreateFeedbackCommand, BaseResponse<FeedbackDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenServices;

        public CreateFeedbackHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenServices = tokenServices;
        }

        public async Task<BaseResponse<FeedbackDto>> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<FeedbackDto> response = new BaseResponse<FeedbackDto>();

            try
            {
                ClaimsPrincipal claims = _tokenServices.ValidateToken(request.Token ?? "");
                if (claims != null)
                {
                    int.TryParse(claims.FindFirst("jti")?.Value, out int userId);

                    var isLecturer = await _unitOfWork.UserRepository.IsUserLecturer(request.LecturerId);
                    if (!isLecturer)
                    {
                        response.Error = true;
                        response.Message = "Invalid lecturer ID";
                        response.Exception = new BadRequestException();
                    }
                    else
                    {
                        var feedbackModel = new FeedbackModel
                        {
                            Time = DateTime.Now,
                            Content = request.Content,
                            StudentId = userId,
                            LecturerId = request.LecturerId
                        };

                        var createdFeedbackId = await _unitOfWork.UserRepository.CreateFeedback(feedbackModel);
                        await _unitOfWork.Save();

                        var feedbackDto = _mapper.Map<FeedbackDto>(feedbackModel);
                        feedbackDto.FeedbackId = createdFeedbackId;

                        response.Result = feedbackDto;
                        response.Message = "Feedback created successfully!";
                    }
                }
                else
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("Invalid credentials");
                }
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
