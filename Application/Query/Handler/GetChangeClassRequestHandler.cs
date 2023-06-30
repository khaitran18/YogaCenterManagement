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
    public class GetChangeClassRequestHandler : IRequestHandler<GetChangeClassRequestsQuery, BaseResponse<IEnumerable<ChangeClassRequestDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetChangeClassRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<IEnumerable<ChangeClassRequestDto>>> Handle(GetChangeClassRequestsQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<ChangeClassRequestDto>>();
            try
            {
                var classRequests = await _unitOfWork.ClassRepository.GetChangeClassRequests();
                var classRequestDtos = _mapper.Map<List<ChangeClassRequestDto>>(classRequests);
                response.Result = classRequestDtos;
                response.Message = "Success";
            }
            catch (Exception e)
            {
                response.Error = true;
                response.Message = e.Message;
                throw;
            }
            return response;
        }
    }
}
