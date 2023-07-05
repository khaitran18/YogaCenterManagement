using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class AvailableDateModel
    {
        public int LecturerId { get; set; }
        public int SlotId { get; set; }
        public DateTime? Date { get; set; }
        public StudySlotModel Slot { get; set; } = null!;
    }
}
