using Application.Common;
using Application.Common.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query
{
    public record UserDetailQuery : IRequest<BaseResponse<UserDto>>
    {
        public string Token { get; set; }
    }
}
