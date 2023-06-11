using System;
using System.Collections.Generic;

namespace Infrastructure.DataModels
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public DateTime? Time { get; set; }
        public string? Content { get; set; }
        public int? StudentId { get; set; }
        public int? LecturerId { get; set; }

        public virtual User? Lecturer { get; set; }
        public virtual User? Student { get; set; }
    }
}
