using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.Handler
{
    public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public VerifyEmailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<bool>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            response.Result = false;
            try
            {
                if (_unitOfWork.UserRepository.VerifyToken(request.t).IsCompletedSuccessfully)
                {
                    response.Result = true;
                }
                else
                {
                    response.Error = true;
                    response.Exception = new BadRequestException("Invalid credential");
                    response.Message = "Invalid credential";
                }
            }
            catch (Exception e)
            {
                response.Error = true;
                response.Exception = e;
                response.Message = "Error in verifying email";
            }
            return response;
        }
    }
}
