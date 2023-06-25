using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public record SignUpCommand : IRequest<BaseResponse<bool>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
