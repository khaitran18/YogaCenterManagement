using Application.Common.Dto;
using Application.Interfaces;
using MediatR;
using Application.Common.Exceptions;
using Application.Common;
using AutoMapper;

namespace Application.Query.Handler
{
    public class GetClassByIdHandler : IRequestHandler<GetClassByIdQuery, BaseResponse<ClassDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClassByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ClassDto>> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
        {
            BaseResponse<ClassDto> response = new BaseResponse<ClassDto>();

            var classModel = await _unitOfWork.ClassRepository.GetClassById(request.ClassId);
            var classDto = _mapper.Map<ClassDto>(classModel);
            response.Result = classDto;
            response.Message = "Get class successfully!";

            return response;
        }
    }
}
