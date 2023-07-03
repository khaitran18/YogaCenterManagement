using Application.Common;
using Application.Common.Dto;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Service;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handler
{
    public class CreateStudySlotHandler : IRequestHandler<CreateStudySlotCommand, BaseResponse<StudySlotDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;
        private readonly IMapper _mapper;
        public CreateStudySlotHandler(IUnitOfWork unitOfWork, ITokenService tokenServices, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }
        public async Task<BaseResponse<StudySlotDto>> Handle(CreateStudySlotCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<StudySlotDto> response = new BaseResponse<StudySlotDto>();
            try
            {
                Console.WriteLine("HEHE" + request.startTime);
                ClaimsPrincipal claims = _tokenServices.ValidateToken(request.token ?? "");
                if (claims != null)
                {
                    if (request.startTime > request.endTime)
                    {
                        response.Error = true;
                        response.Exception = new BadRequestException("Start time can not be greater than end time");
                    }
                    else
                    {
                        var studySlot = await _unitOfWork.ScheduleRepository.CreateSlot(request.startTime, request.endTime, request.dateIds);
                        response.Result = new StudySlotDto { StartTime = studySlot.StartTime, EndTime = studySlot.EndTime, Day = _mapper.Map<List<DayDto>>(studySlot.Day) };
                    }
                }
                else
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("Invalid credential");
                }

            }
            catch (Exception e)
            {
                response.Error = true;
                response.Exception = e;
            }
            return response;
        }
    }
}
