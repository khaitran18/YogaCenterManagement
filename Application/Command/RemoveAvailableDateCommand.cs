using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class RemoveAvailableDateCommand : IRequest<BaseResponse<bool>>
    {
        public int LecturerId { get; set; }
        public int SlotId { get; set; }
    }
}
