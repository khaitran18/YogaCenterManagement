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
    public class StudentEnrollToClassCommand : IRequest<BaseResponse<PaymentDto>>
    {
        public PaymentDto PaymentDto { get; set; }
    }
}
