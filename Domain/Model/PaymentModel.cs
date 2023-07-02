using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class PaymentModel
    {
        public int PaymentId { get; set; }
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public decimal Amount { get; set; }
        public string? Method { get; set; }

        public virtual UserModel? Student { get; set; }
        public virtual ClassModel? Class { get; set; }
    }
}
