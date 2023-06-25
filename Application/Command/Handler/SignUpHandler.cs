using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Service;
using MediatR;

namespace Application.Command.Handler
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public SignUpHandler(IUnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }

        public async Task<BaseResponse<bool>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            response.Result = false;
            try
            {
                //check if username exist
                if (!await _unitOfWork.UserRepository.ExistUserName(request.UserName))
                {
                    //signup for the account
                    response.Result = await _unitOfWork.UserRepository.Create(request.UserName, request.Password, request.Phone, request.FullName, request.Address);

                    //send email verification
                    if (response.Result)
                    {
                        //
                    }
                    else
                    {
                        response.Error = true;
                        response.Exception = new Exception();
                        response.Message = "Error in creating new account";
                    }
                }
                else
                {
                    response.Error = true;
                    response.Exception = new BadRequestException();
                    response.Message = "Username exist";
                }
            }
            catch (Exception e)
            {
                response.Error = true;
                response.Exception = e;
                response.Message = e.Message;
            }
            return response;
        }
    }
}
