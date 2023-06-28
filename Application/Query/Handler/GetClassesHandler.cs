using Application.Common.Dto;
using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;
using Application.Common;
using AutoMapper;

namespace Application.Query.Handler
{
    public class GetClassesHandler : IRequestHandler<GetClassesQuery, BaseResponse<PaginatedResult<ClassDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClassesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<PaginatedResult<ClassDto>>> Handle(GetClassesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaginatedResult<ClassDto>>();

            try
            {
                var (classModels, totalCount) = await _unitOfWork.ClassRepository.GetClasses(
                    request.SearchKeyword,
                    request.SortBy,
                    request.StartingFromDate,
                    request.DurationMonths,
                    request.ClassCapacity,
                    request.Page,
                    request.PageSize
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
                response.Message = "An error occurred while fetching classes.";
                response.Exception = ex;
            }

            return response;
        }
    }
}
