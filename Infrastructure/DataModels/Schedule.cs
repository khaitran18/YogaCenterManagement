using System;
using System.Collections.Generic;

namespace Infrastructure.DataModels
{
    public partial class Schedule
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public int? SlotId { get; set; }
        public DateTime Date { get; set; }
        public string? DailyNote { get; set; }

        public virtual Class? Class { get; set; }
        public virtual StudySlot? Slot { get; set; }
    }
}
