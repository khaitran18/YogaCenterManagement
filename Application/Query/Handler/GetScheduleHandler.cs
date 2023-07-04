using Application.Common;
using Application.Common.Dto;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Service;
using AutoMapper;
using Domain.Model;
using MediatR;

namespace Application.Query.Handler
{
    public class GetScheduleHandler : IRequestHandler<GetScheduleQuery, BaseResponse<List<ScheduleDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public GetScheduleHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<BaseResponse<List<ScheduleDto>>> Handle(GetScheduleQuery request, CancellationToken cancellationToken)
        {
            BaseResponse<List<ScheduleDto>> response = new BaseResponse<List<ScheduleDto>>();
            try
            {
                if (request.token == null)
                {
                    response.Error = true;
                    response.Exception = new ForbiddenAccessException();
                    response.Message = "Error";
                }
                else
                {
                    var claims = _tokenService.ValidateToken(request.token);
                    if (int.TryParse(claims.FindFirst("jti").Value,out int userId))
                    {
                        ClassModel @class = null;
                        if (claims.IsInRole("User"))
                        {
                            @class = await _unitOfWork.ClassRepository.GetStudyingClassByClassId(userId, request.classId);
                            
                        }
                        else if (claims.IsInRole("Lecturer"))
                        {
                            @class = await _unitOfWork.ClassRepository.GetTeachingClassByClassId(userId, request.classId);
                        }
                        if (@class != null)
                        {
                            List<ScheduleModel> list = await _unitOfWork.ScheduleRepository.GetScheduleByStartDateAndEndDateAndClassId(request.startDate, request.endDate, request.classId);
                            if (list != null) response.Result = _mapper.Map<List<ScheduleDto>>(list);
                        }
                        else
                        {
                            response.Error = true;
                            response.Exception = new BadRequestException("No current class found");
                        }
                    }
                    else
                    {
                        response.Error = true;
                        response.Exception = new BadRequestException("Invalid crefidentials");
                    }

                }
            }
            catch (Exception e)
            {
                response.Error = true;
                response.Message = e.Message;
                response.Exception = e;
            }
            return response;
        }
    }
}
