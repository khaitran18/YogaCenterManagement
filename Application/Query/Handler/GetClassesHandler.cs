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
    public class GetClassesHandler : IRequestHandler<GetClassesQuery, BaseResponse<PaginatedResult<ClassDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public GetClassesHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<BaseResponse<PaginatedResult<ClassDto>>> Handle(GetClassesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaginatedResult<ClassDto>>();
            try
            {
                ClaimsPrincipal? claims = _tokenService.ValidateToken(request.Token ?? "");
                int userId = 0;
                bool isAdmin = false;

                if (claims != null)
                {
                    int.TryParse(claims.FindFirst("jti")?.Value, out userId);
                    isAdmin = await _unitOfWork.UserRepository.IsUserAdmin(userId);
                }

                var (classModels, totalCount) = await _unitOfWork.ClassRepository.GetClasses(
                    request.SearchKeyword,
                    request.SortBy,
                    request.StartingFromDate,
                    request.DurationMonths,
                    request.ClassCapacity,
                    request.Page,
                    request.PageSize,
                    isAdmin
                );

                var classDtos = classModels.Select(c => _mapper.Map<ClassDto>(c)).ToList();

                var paginatedResult = new PaginatedResult<ClassDto>(
                    classDtos,
                    totalCount,
                    request.Page,
                    request.PageSize
                );

                response.Result = paginatedResult;
                response.Message = "Get classes successfully!";
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
