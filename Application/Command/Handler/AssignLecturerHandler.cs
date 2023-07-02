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
    public class AssignLecturerHandler : IRequestHandler<AssignLecturerCommand, BaseResponse<ClassDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;
        public AssignLecturerHandler(IUnitOfWork unitOfWork, ITokenService tokenServices)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
        }
        public async Task<BaseResponse<ClassDto>> Handle(AssignLecturerCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<ClassDto> response = new BaseResponse<ClassDto>();
            try
            {

                ClaimsPrincipal claims = _tokenServices.ValidateToken(request.token ?? "");
                if (claims != null)
                {
                    
                    var newClass = await _unitOfWork.ClassRepository.AssignLecturer(request.ClassId,request.LecturerId);
                    response.Result = new ClassDto
                    {
                        ClassId = newClass.ClassId,
                        ClassName = newClass.ClassName,
                        ClassCapacity = newClass.ClassCapacity,
                        StartDate = newClass.StartDate,
                        EndDate = newClass.EndDate,
                        Price = newClass.Price,
                        LecturerId = newClass.LecturerId,
                    };
                    response.Message = "Assign lecturer successfully";
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
