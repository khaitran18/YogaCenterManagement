using System;
using System.Collections.Generic;

namespace Infrastructure.DataModels
{
    public partial class Class
    {
        public Class()
        {
            ChangeClassRequestClasses = new HashSet<ChangeClassRequest>();
            ChangeClassRequestRequestClasses = new HashSet<ChangeClassRequest>();
            Payments = new HashSet<Payment>();
            Posts = new HashSet<Post>();
            Schedules = new HashSet<Schedule>();
            Students = new HashSet<User>();
        }

        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? LecturerId { get; set; }
        public double Price { get; set; }
        public int ClassCapacity { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public short? ClassStatus { get; set; }

        public virtual User? Lecturer { get; set; }
        public virtual ICollection<ChangeClassRequest> ChangeClassRequestClasses { get; set; }
        public virtual ICollection<ChangeClassRequest> ChangeClassRequestRequestClasses { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }

        public virtual ICollection<User> Students { get; set; }
    }
}
