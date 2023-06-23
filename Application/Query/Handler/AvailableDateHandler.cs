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
    public class AvailableDateHandler : IRequestHandler<AvailableDateQuery, BaseResponse<IEnumerable<AvailableDateDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AvailableDateHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<AvailableDateDto>>> Handle(AvailableDateQuery request, CancellationToken cancellationToken)
        {
            BaseResponse<IEnumerable<AvailableDateDto>> response = new BaseResponse<IEnumerable<AvailableDateDto>>();
            try
            {
                var availableDateModels = await _unitOfWork.ScheduleRepository.GetAvailableDatesBySlotId(request.SlotId);
                var availableDates = new List<AvailableDateDto>();
                availableDates = _mapper.Map<List<AvailableDateDto>>(availableDateModels);
                response.Result = availableDates;
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
    }
}
