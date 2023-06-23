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
    public class AddAvailableDateCommand : IRequest<BaseResponse<IEnumerable<AvailableDateDto>>>
    {
        public string? Token { get; set; }
        public int LecturerId { get; set; }
        public List<int> SlotIds { get; set; } = new List<int>();
    }
}
