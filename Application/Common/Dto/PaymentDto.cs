using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dto
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public decimal Amount { get; set; }
        public string? Method { get; set; }

        public virtual UserDto? Student { get; set; }
        public virtual ClassDto? Class { get; set; }
    }
}
