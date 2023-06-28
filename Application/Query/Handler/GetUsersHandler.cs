using Application.Common.Dto;
using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;
using Application.Common;
using AutoMapper;

namespace Application.Query.Handler
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, BaseResponse<List<UserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUsersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<UserDto>>();

            try
            {
                var users = await _unitOfWork.UserRepository.GetAll();
                var userDtos = _mapper.Map<List<UserDto>>(users);

                response.Result = userDtos;
                response.Message = "Get all users successfully!";
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
