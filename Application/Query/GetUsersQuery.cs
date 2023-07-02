using Application.Common;
using Application.Common.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Query
{
    public class GetUsersQuery : IRequest<BaseResponse<PaginatedResult<UserDto>>>
    {
        public string? Token { get; set; }
        public int? RoleId { get; set; }
        public bool? IsVerified { get; set; }
        public bool? IsDisabled { get; set; }
        public string? SortBy { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
