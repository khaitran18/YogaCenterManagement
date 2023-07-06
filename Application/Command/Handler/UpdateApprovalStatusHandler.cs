using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handler
{
    public class UpdateApprovalStatusHandler : IRequestHandler<UpdateApprovalStatusCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenServices;
        public UpdateApprovalStatusHandler(IUnitOfWork unitOfWork, ITokenService tokenServices)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
        }
        public async Task<BaseResponse<bool>> Handle(UpdateApprovalStatusCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var result = await _unitOfWork.ClassRepository.UpdateApprovalStatus(request.requestId, request.isApproved);
                if (!result)
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("Fail to update request");
                }
                else
                {
                    response.Result = result;
                    response.Message = "Success";
                }
            }
            catch (Exception)
            {
                response.Error = true;
                response.Message = "Fail to update status";
                throw;
            }
            return response;
        }
    }
}
