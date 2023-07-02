using Application.Common;
using Application.Common.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.Handler
{
    public class GetChangeClassHandler : IRequestHandler<GetChangeClassQuery, BaseResponse<IEnumerable<ClassDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetChangeClassHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<IEnumerable<ClassDto>>> Handle(GetChangeClassQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<ClassDto>>();
            try
            {
                var classes = await _unitOfWork.ClassRepository.GetChangeClasses(request.FromClassId);
                response.Result = _mapper.Map<List<ClassDto>>(classes);
                
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
