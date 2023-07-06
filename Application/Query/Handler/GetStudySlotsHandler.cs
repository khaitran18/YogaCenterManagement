using Application.Common;
using Application.Common.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetStudySlotsHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<IEnumerable<StudySlotDto>>> Handle(GetStudySlotsQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<StudySlotDto>>();
            try
            {
                var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

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
