﻿using Application.Common;
using Application.Common.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.Handler
{
    public class GetStudiedClassHandler : IRequestHandler<GetStudiedClassQuery, BaseResponse<PaginatedResult<ClassDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudiedClassHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<PaginatedResult<ClassDto>>> Handle(GetStudiedClassQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<PaginatedResult<ClassDto>>();
            try
            {
                var (classModels, totalCount) = await _unitOfWork.ClassRepository.GetStudiedClass(request.StudentId, request.Page, request.PageSize);
                var classDtos = _mapper.Map<List<ClassDto>>(classModels);
                var paginatedResult = new PaginatedResult<ClassDto>(
                                                                    classDtos,
                                                                    totalCount,
                                                                    request.Page,
                                                                    request.PageSize
                                                                );
                response.Result = paginatedResult;
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