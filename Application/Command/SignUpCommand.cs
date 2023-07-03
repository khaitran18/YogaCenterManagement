using Application.Common;
using Application.Common.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public record SignUpCommand : IRequest<BaseResponse<UserDto>>
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "User";
        public string? Token { get; set; }
    }
}
