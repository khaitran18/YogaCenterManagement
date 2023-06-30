﻿using Application.Common;
using Application.Common.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class DisableUserCommand : IRequest<BaseResponse<UserDto>>
    {
        public int UserId { get; set; }
        public string Reason { get; set; } = null!;
    }
}
