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
    public class CreateChangeRequestCommand : IRequest<BaseResponse<ClassDto>>
    {
        public int FromClassId { get; set; }
        public int ToClassId { get; set; }
        public int StudentId { get; set; }
        public string Content { get; set; }

    }
}
