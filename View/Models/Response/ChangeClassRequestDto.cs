using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models.Response
{
    public class ChangeClassRequestDto
    {
        public int RequestId { get; set; }
        public int? UserId { get; set; }
        public int? ClassId { get; set; }
        public string? Content { get; set; }
        public bool IsApproved { get; set; }
        public int? RequestClassId { get; set; }

        public ClassDto? Class { get; set; }
        public ClassDto? RequestClass { get; set; }
        public UserDto? User { get; set; }
    }
}
