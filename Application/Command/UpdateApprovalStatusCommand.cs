using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class UpdateApprovalStatusCommand : IRequest<BaseResponse<bool>>
    {
        public int requestId { get; set; }
        public bool isApproved { get; set; }

    }
}
