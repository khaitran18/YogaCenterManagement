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
    public class AssignLecturerCommand : IRequest<BaseResponse<ClassDto>>
    {
        public string? token { get; set; }
        public int ClassId { get; set; }
        public int LecturerId { get; set; }
    }
}
