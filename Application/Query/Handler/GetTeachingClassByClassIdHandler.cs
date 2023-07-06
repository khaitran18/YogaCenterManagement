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

namespace Application.Query.Handler
{
    public class GetTeachingClassByClassIdHandler : IRequestHandler<GetTeachingClassByClassIdQuery, BaseResponse<ClassDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenServices;
        public GetTeachingClassByClassIdHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ITokenService tokenServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _tokenServices = tokenServices;
        }
        public async Task<BaseResponse<ClassDto>> Handle(GetTeachingClassByClassIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<ClassDto>();
            try
            {
                string authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                ClaimsPrincipal claims = _tokenServices.ValidateToken(authorizationHeader);
                int lecturerId = int.Parse(claims.FindFirst("jti")?.Value ?? "0");
                if (lecturerId != request.LecturerId)
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("Student id not match");
                }
                else
                {
                    var classModel = await _unitOfWork.ClassRepository.GetTeachingClassByClassId(request.LecturerId, request.ClassId);
                    response.Result = _mapper.Map<ClassDto>(classModel);
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
