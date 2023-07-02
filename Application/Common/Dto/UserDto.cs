using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dto
{
    public class UserDto
    {
        public int Uid { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public int? RoleId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDisabled { get; set; }
        public string Email { get; set; } = null!;
    }
}
