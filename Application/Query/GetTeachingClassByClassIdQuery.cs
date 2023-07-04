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
    public class GetTeachingClassByClassIdQuery : IRequest<BaseResponse<ClassDto>>
    {
        public int LecturerId { get; set; }
        public int ClassId { get; set; }
    }
}
