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
    public class GetChangeClassRequestsQuery : IRequest<BaseResponse<IEnumerable<ChangeClassRequestDto>>>
    {
    }
}
