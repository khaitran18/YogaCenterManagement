using System;
using System.Collections.Generic;

namespace Infrastructure.DataModels
{
    public partial class Class
    {
        public Class()
        {
            Schedules = new HashSet<Schedule>();
            Students = new HashSet<User>();
        }

        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? LecturerId { get; set; }

        public virtual User? Lecturer { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }

        public virtual ICollection<User> Students { get; set; }
    }
}
