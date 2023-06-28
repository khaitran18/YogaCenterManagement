using Application.Common;
using Application.Common.Dto;
using MediatR;

namespace Application.Command
{
    public class AuthCommand : IRequest<BaseResponse<AuthResponseDto>>
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
