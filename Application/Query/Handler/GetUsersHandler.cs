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
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, BaseResponse<PaginatedResult<UserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public GetUsersHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<BaseResponse<PaginatedResult<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaginatedResult<UserDto>>();

            try
            {
                ClaimsPrincipal claims = _tokenService.ValidateToken(request.Token ?? "");
                if (claims != null)
                {
                    int.TryParse(claims.FindFirst("jti")?.Value, out int userId);

                    var isAdmin = await _unitOfWork.UserRepository.IsUserAdmin(userId);

                    var (userModels, totalCount) = await _unitOfWork.UserRepository.GetUsers(
                        request.RoleIds,
                        request.IsDisabled,
                        request.IsVerified,
                        request.SortBy,
                        request.Page,
                        request.PageSize,
                        isAdmin
                    );

                    var userDtos = _mapper.Map<List<UserDto>>(userModels);

                    var paginatedResult = new PaginatedResult<UserDto>(
                        userDtos,
                        totalCount,
                        request.Page,
                        request.PageSize
                    );

                    response.Result = paginatedResult;
                    response.Message = "Get users successfully!";
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
