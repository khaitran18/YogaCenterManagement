using Application.Common.Dto;
using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;
using Application.Common;
using AutoMapper;
using System.Security.Claims;
using Application.Service;

namespace Application.Query.Handler
{
    public class GetFeedbacksHandler : IRequestHandler<GetFeedbacksQuery, BaseResponse<PaginatedResult<FeedbackDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public GetFeedbacksHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<BaseResponse<PaginatedResult<FeedbackDto>>> Handle(GetFeedbacksQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaginatedResult<FeedbackDto>>();

            try
            {
                ClaimsPrincipal? claims = _tokenService.ValidateToken(request.Token ?? "");
                if (claims != null)
                {
                    int.TryParse(claims.FindFirst("jti")?.Value, out int userId);

                    var isLecturer = await _unitOfWork.UserRepository.IsUserLecturer(userId);

                    var (feedbackModels, totalCount) = await _unitOfWork.UserRepository.GetFeedbacks(
                        userId,
                        isLecturer,
                        request.SortBy,
                        request.Page,
                        request.PageSize
                    );

                    var feedbackDtos = _mapper.Map<List<FeedbackDto>>(feedbackModels);

                    var paginatedResult = new PaginatedResult<FeedbackDto>(
                        feedbackDtos,
                        totalCount,
                        request.Page,
                        request.PageSize
                    );

                    response.Result = paginatedResult;
                    response.Message = "Get feedbacks successfully!";
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
