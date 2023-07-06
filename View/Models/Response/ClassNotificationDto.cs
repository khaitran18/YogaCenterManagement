using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models.Response
{
    public class ClassNotificationDto
    {
        public string? Content { get; set; }
        public int ScheduleId { get; set; }
        public int ClassId { get; set; }
    }
}
