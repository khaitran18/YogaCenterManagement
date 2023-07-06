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
    public class GetAvailableDateByLecturerIdHandler : IRequestHandler<GetAvailableDateByLecturerId, BaseResponse<IEnumerable<AvailableDateDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenServices;
        public GetAvailableDateByLecturerIdHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ITokenService tokenServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _tokenServices = tokenServices;
        }
        public async Task<BaseResponse<IEnumerable<AvailableDateDto>>> Handle(GetAvailableDateByLecturerId request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<AvailableDateDto>>();
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
                    var availabeDates = await _unitOfWork.ScheduleRepository.GetAvailableDatesByLecturerId(request.LecturerId);
                    var availableDateDtos = _mapper.Map<List<AvailableDateDto>>(availabeDates);
                    response.Result = availableDateDtos;
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
