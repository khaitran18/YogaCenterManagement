using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models.Response
{
    public class AvailableDateDto
    {
        public int LecturerId { get; set; }
        public int SlotId { get; set; }
        public DateTime? Date { get; set; }
        public StudySlotDto Slot { get; set; } = null!;
    }
}
