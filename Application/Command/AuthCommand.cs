using Application.Common.Dto;
using MediatR;

namespace Application.Command
{
    public class AuthCommand : IRequest<AuthResponseDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
