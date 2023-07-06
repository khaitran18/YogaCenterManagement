using Application.Common;
using Application.Common.Dto;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query
{
    public class GetClassesQuery : IRequest<BaseResponse<PaginatedResult<ClassDto>>>
    {
        public string? Token { get; set; }
        public string? SearchKeyword { get; set; }
        public string? SortBy { get; set; }
        public DateTime? StartingFromDate { get; set; }
        public int? DurationMonths { get; set; }
        public string? ClassCapacity { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
