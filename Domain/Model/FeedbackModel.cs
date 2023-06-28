using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class FeedbackModel
    {
        public int FeedbackId { get; set; }
        public DateTime? Time { get; set; }
        public string? Content { get; set; }
        public int? StudentId { get; set; }
        public int? LecturerId { get; set; }
        public UserModel? Lecturer { get; set; }
        public UserModel? Student { get; set; }
    }
}
