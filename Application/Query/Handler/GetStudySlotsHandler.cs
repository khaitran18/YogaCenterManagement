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
    public class GetStudySlotsHandler : IRequestHandler<GetStudySlotsQuery, BaseResponse<IEnumerable<StudySlotDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetStudySlotsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<IEnumerable<StudySlotDto>>> Handle(GetStudySlotsQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<StudySlotDto>>();
            try
            {
                var studySlotModels = await _unitOfWork.ScheduleRepository.GetAllStudySlot();
                response.Result = _mapper.Map<List<StudySlotDto>>(studySlotModels);
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
