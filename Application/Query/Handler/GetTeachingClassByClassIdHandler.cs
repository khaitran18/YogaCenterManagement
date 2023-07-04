using Application.Common;
using Application.Common.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.Handler
{
    public class GetTeachingClassByClassIdHandler : IRequestHandler<GetTeachingClassByClassIdQuery, BaseResponse<ClassDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTeachingClassByClassIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<ClassDto>> Handle(GetTeachingClassByClassIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<ClassDto>();
            try
            {
                var classModel = await _unitOfWork.ClassRepository.GetTeachingClassByClassId(request.LecturerId, request.ClassId);
                response.Result = _mapper.Map<ClassDto>(classModel);
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
