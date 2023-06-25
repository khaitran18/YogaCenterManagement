using Application.Common;
using Application.Common.Dto;
using MediatR;

namespace Application.Query
{
    public class GetClassByIdQuery : IRequest<BaseResponse<ClassDto>>
    {
        public int ClassId { get; set; }
    }
}
