using Application.Common;
using Application.Common.Dto;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Service;
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
        public CreateStudySlotHandler(IUnitOfWork unitOfWork, ITokenService tokenServices)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
        }
        public async Task<BaseResponse<StudySlotDto>> Handle(CreateStudySlotCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<StudySlotDto> response = new BaseResponse<StudySlotDto>();
            try
            {

                ClaimsPrincipal claims = _tokenServices.ValidateToken(request.token ?? "");
                if (claims != null)
                {
                    var studySlot = await _unitOfWork.ScheduleRepository.CreateSlot(request.startTime, request.endTime, request.dateIds);
                    response.Result = new StudySlotDto { StartTime = studySlot.StartTime, EndTime = studySlot.EndTime, Day = studySlot.Day };
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
