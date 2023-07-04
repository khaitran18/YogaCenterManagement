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
                    if(int.TryParse(_tokenService.ValidateToken(request.token).FindFirst("jti").Value,out int userId))
                    {
                        var @class = await _unitOfWork.ClassRepository.GetStudyingClassByClassId(userId, request.classId);
                        if (@class != null)
                        {
                            List<ScheduleModel> list = await _unitOfWork.ScheduleRepository.GetScheduleByStartDateAndEndDate(request.startDate, request.endDate);
                            if (list!=null) response.Result = _mapper.Map<List<ScheduleDto>>(list);
                        }
                        else
                        {
                            response.Error = true;
                            response.Exception = new BadRequestException("No current class found");
                            response.Message = "No current class found";
                        }
                    }
                    else
                    {
                        response.Error = true;
                        response.Exception = new BadRequestException("Invalid crefidentials");
                        response.Message = "Error";
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
