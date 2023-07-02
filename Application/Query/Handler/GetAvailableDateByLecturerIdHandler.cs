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
    public class GetAvailableDateByLecturerIdHandler : IRequestHandler<GetAvailableDateByLecturerId, BaseResponse<IEnumerable<AvailableDateDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAvailableDateByLecturerIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<IEnumerable<AvailableDateDto>>> Handle(GetAvailableDateByLecturerId request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<AvailableDateDto>>();
            try
            {
                var availabeDates = await _unitOfWork.ScheduleRepository.GetAvailableDatesByLecturerId(request.LecturerId);
                var availableDateDtos = _mapper.Map<List<AvailableDateDto>>(availabeDates);
                response.Result = availableDateDtos;
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
