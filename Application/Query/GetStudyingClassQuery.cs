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
    public class GetStudyingClassQuery : IRequest<BaseResponse<PaginatedResult<ClassDto>>>
    {
        public int StudentId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
