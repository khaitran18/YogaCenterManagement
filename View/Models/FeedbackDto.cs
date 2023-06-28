using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; }
        public DateTime? Time { get; set; }
        public string? Content { get; set; }
        public int? StudentId { get; set; }
        public int? LecturerId { get; set; }
        public UserDto? Lecturer { get; set; }
        public UserDto? Student { get; set; }
    }
}
