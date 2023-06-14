using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class AuthCommand : IRequest<AuthResponseDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
