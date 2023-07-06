using Application.Common;
using Application.Common.Dto;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Service;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handler
{
    public class CreateChangeRequestClassHandler : IRequestHandler<CreateChangeRequestCommand, BaseResponse<ClassDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateChangeRequestClassHandler(IUnitOfWork unitOfWork, ITokenService tokenServices, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<ClassDto>> Handle(CreateChangeRequestCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<ClassDto>();
            try
            {
                string authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                ClaimsPrincipal claims = _tokenServices.ValidateToken(authorizationHeader);
                int studentId = int.Parse(claims.FindFirst("jti")?.Value ?? "0");
                if (studentId != request.StudentId)
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("Student id not match");
                }
                else
                {

                    if (await _unitOfWork.ClassRepository.ExistChangeClassRequest(request.StudentId, request.FromClassId, request.ToClassId))
                    {
                        response.Error = true;
                        response.Message = "Same request already exist";
                    }
                    else
                    {
                        var classModel = await _unitOfWork.ClassRepository.RequestChangeClass(request.FromClassId, request.StudentId, request.ToClassId, request.Content);
                        var classDto = _mapper.Map<ClassDto>(classModel);
                        response.Result = classDto;
                    }
                }

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
