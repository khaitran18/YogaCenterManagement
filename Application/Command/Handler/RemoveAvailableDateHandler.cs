using Application.Common;
using Application.Interfaces;
using Application.Service;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handler
{
    public class RemoveAvailableDateHandler : IRequestHandler<RemoveAvailableDateCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;
        private readonly IMapper _mapper;
        public RemoveAvailableDateHandler(IUnitOfWork unitOfWork, ITokenService tokenServices, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }
        public async Task<BaseResponse<bool>> Handle(RemoveAvailableDateCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var check = await _unitOfWork.ScheduleRepository.RemoveLecturerAvailableDate(request.LecturerId, request.SlotId);
                response.Result = check;
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
