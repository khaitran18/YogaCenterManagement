using System;
using System.Collections.Generic;

namespace Infrastructure.DataModels
{
    public partial class User
    {
        public User()
        {
            AvailableDates = new HashSet<AvailableDate>();
            ChangeClassRequests = new HashSet<ChangeClassRequest>();
            Classes = new HashSet<Class>();
            FeedbackLecturers = new HashSet<Feedback>();
            FeedbackStudents = new HashSet<Feedback>();
            Payments = new HashSet<Payment>();
            ClassesNavigation = new HashSet<Class>();
        }

        public int Uid { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public int? RoleId { get; set; }
        public bool IsVerified { get; set; } = false;

        public virtual Role? Role { get; set; }
        public virtual ICollection<AvailableDate> AvailableDates { get; set; }
        public virtual ICollection<ChangeClassRequest> ChangeClassRequests { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Feedback> FeedbackLecturers { get; set; }
        public virtual ICollection<Feedback> FeedbackStudents { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

        public virtual ICollection<Class> ClassesNavigation { get; set; }
    }
}
