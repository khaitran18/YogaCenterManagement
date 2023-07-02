using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class StudySlotModel
    {
        public int SlotId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<DayModel> Day { get; set; } = new List<DayModel>();
    }
}
