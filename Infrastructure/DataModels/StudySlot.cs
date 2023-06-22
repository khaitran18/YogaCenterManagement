using System;
using System.Collections.Generic;

namespace Infrastructure.DataModels
{
    public partial class StudySlot
    {
        public StudySlot()
        {
            AvailableDates = new HashSet<AvailableDate>();
            Schedules = new HashSet<Schedule>();
            Days = new HashSet<DateOfWeek>();
        }

        public int SlotId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public virtual ICollection<AvailableDate> AvailableDates { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }

        public virtual ICollection<DateOfWeek> Days { get; set; }
    }
}
