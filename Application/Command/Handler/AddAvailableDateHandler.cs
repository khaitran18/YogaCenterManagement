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
    public class AddAvailableDateHandler : IRequestHandler<AddAvailableDateCommand, BaseResponse<IEnumerable<AvailableDateDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;
        private readonly IMapper _mapper;
        public AddAvailableDateHandler(IUnitOfWork unitOfWork, ITokenService tokenServices, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }
        public async Task<BaseResponse<IEnumerable<AvailableDateDto>>> Handle(AddAvailableDateCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<IEnumerable<AvailableDateDto>> response = new BaseResponse<IEnumerable<AvailableDateDto>>();
            try
            {

                ClaimsPrincipal claims = _tokenServices.ValidateToken(request.Token ?? "");
                if (claims != null)
                {
                    int lecturerIdToken = int.Parse(claims.FindFirst("jti")?.Value ?? "0");
                    if (lecturerIdToken != request.LecturerId)
                    {
                        response.Error = true;
                        response.Exception = new BadRequestException("Lecturer id not match");
                    }
                    else
                    {
                        var availableDateModels = await _unitOfWork.ScheduleRepository.AddAvailableDate(request.LecturerId, request.SlotIds);
                        var availableDateDtos = new List<AvailableDateDto>();
                        foreach (var availableDateModel in availableDateModels)
                        {
                            availableDateDtos.Add(new AvailableDateDto { LecturerId = availableDateModel.LecturerId, SlotId = availableDateModel.SlotId, Slot = new StudySlotDto { StartTime = availableDateModel.Slot.StartTime, EndTime = availableDateModel.Slot.EndTime, Day = _mapper.Map<List<DayDto>>(availableDateModel.Slot.Day) } });
                        }
                        response.Result = availableDateDtos;
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
