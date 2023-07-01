using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models.Response
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public int? SlotId { get; set; }
        public DateTime Date { get; set; }
        public string DailyNote { get; set; }
        public StudySlotDto Slot { get; set; }
    }
}
