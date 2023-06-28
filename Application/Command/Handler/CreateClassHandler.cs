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
    public class CreateClassHandler : IRequestHandler<CreateClassCommand, BaseResponse<ClassDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;
        public CreateClassHandler(IUnitOfWork unitOfWork, ITokenService tokenServices)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
        }
        public async Task<BaseResponse<ClassDto>> Handle(CreateClassCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<ClassDto> response = new BaseResponse<ClassDto>();
            try
            {

                ClaimsPrincipal claims = _tokenServices.ValidateToken(request.token ?? "");
                if (claims != null)
                {
                    List<int> dateIds = new List<int>();
                    if(request.SelectedDayOfWeek != null)
                    {
                        foreach (string day in request.SelectedDayOfWeek.Split(','))
                        {
                            dateIds.Add(int.Parse(day));
                        }
                    }
                    var newClass = await _unitOfWork.ClassRepository.CreateClassSchedule(request.ClassName, request.Price, request.ClassCapacity, request.StartDate,request.EndDate,dateIds);
                    response.Result = new ClassDto { 
                        ClassId = newClass.ClassId, 
                        ClassName = newClass.ClassName, 
                        ClassCapacity = newClass.ClassCapacity,
                        StartDate = newClass.StartDate,
                        EndDate = newClass.EndDate,
                        Price = newClass.Price
                    };
                    response.Message = newClass.Schedules != null ? "Schedules generated" : "No schedule yet";
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
