﻿using Application.Common;
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
    public class GetTeachingClassHandler : IRequestHandler<GetTeachingClassQuery, BaseResponse<PaginatedResult<ClassDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenServices;
        public GetTeachingClassHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ITokenService tokenServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _tokenServices = tokenServices;
        }
        public async Task<BaseResponse<PaginatedResult<ClassDto>>> Handle(GetTeachingClassQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaginatedResult<ClassDto>>();
            try
            {
                string authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                ClaimsPrincipal claims = _tokenServices.ValidateToken(authorizationHeader);
                int lecturerId = int.Parse(claims.FindFirst("jti")?.Value ?? "0");
                if (lecturerId != request.LecturerId)
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("Lecturer id not match");
                }
                else
                {
                    var (classModels, totalCount) = await _unitOfWork.ClassRepository.GetTeachingClass(request.LecturerId, request.Page, request.PageSize);
                    var classDtos = _mapper.Map<List<ClassDto>>(classModels);
                    var paginatedResult = new PaginatedResult<ClassDto>(
                                                                        classDtos,
                                                                        totalCount,
                                                                        request.Page,
                                                                        request.PageSize
                                                                    );
                    response.Result = paginatedResult;
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
