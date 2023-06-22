using System;
using System.Collections.Generic;

namespace Infrastructure.DataModels
{
    public partial class ChangeClassRequest
    {
        public int RequestId { get; set; }
        public int? UserId { get; set; }
        public int? ClassId { get; set; }
        public string? Content { get; set; }
        public bool IsApproved { get; set; }
        public int? RequestClassId { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Class? RequestClass { get; set; }
        public virtual User? User { get; set; }
    }
}
