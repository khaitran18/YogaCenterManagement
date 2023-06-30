using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ChangeClassRequestModel
    {
        public int RequestId { get; set; }
        public int? UserId { get; set; }
        public int? ClassId { get; set; }
        public string? Content { get; set; }
        public bool IsApproved { get; set; }
        public int? RequestClassId { get; set; }

        public ClassModel? Class { get; set; }
        public ClassModel? RequestClass { get; set; }
        public  UserModel? User { get; set; }
    }
}
