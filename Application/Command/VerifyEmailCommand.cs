using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public record class VerifyEmailCommand : IRequest<BaseResponse<bool>>
    {
        public string t { get; set; } = null!;
    }
}
