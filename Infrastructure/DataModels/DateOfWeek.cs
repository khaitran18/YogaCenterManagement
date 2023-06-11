using System;
using System.Collections.Generic;

namespace Infrastructure.DataModels
{
    public partial class DateOfWeek
    {
        public DateOfWeek()
        {
            Slots = new HashSet<StudySlot>();
        }

        public int DayId { get; set; }
        public string Day { get; set; } = null!;

        public virtual ICollection<StudySlot> Slots { get; set; }
    }
}
