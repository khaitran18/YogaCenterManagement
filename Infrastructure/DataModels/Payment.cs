using System;
using System.Collections.Generic;

namespace Infrastructure.DataModels
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int? StudentId { get; set; }
        public decimal Amount { get; set; }
        public string? Method { get; set; }

        public virtual User? Student { get; set; }
    }
}
