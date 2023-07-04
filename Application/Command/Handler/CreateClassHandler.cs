using Application.Common;
using Application.Common.CloudStorage;
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
    public class CreateClassHandler : IRequestHandler<CreateClassCommand, BaseResponse<ClassDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;
        private readonly IMapper _mapper;
        private readonly ICloudStorageService _cloudStorageService;
        public CreateClassHandler(IUnitOfWork unitOfWork, ITokenService tokenServices, IMapper mapper, ICloudStorageService cloudStorageService)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
            _mapper = mapper;
            _cloudStorageService = cloudStorageService;
        }
        public async Task<BaseResponse<ClassDto>> Handle(CreateClassCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<ClassDto> response = new BaseResponse<ClassDto>();
            try
            {

                ClaimsPrincipal claims = _tokenServices.ValidateToken(request.token ?? "");
                if (claims != null)
                {   
                    string ImageUrl = "";
                    if(request.Image != null)
                    {
                        ImageUrl = await _cloudStorageService.UploadFileAsync(request.Image, "image/" + request.ClassName);
                    }
                    var newClass = await _unitOfWork.ClassRepository.CreateClassSchedule(request.ClassName, request.Price, request.ClassCapacity,request.Description,ImageUrl, request.StartDate,request.EndDate,request.SlotId);
                    var classDto = _mapper.Map<ClassDto>(newClass);
                    response.Result = classDto;
                    response.Message = newClass.Schedules != null || newClass.Schedules.Count > 0 ? "Schedules generated" : "No schedule yet";
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
