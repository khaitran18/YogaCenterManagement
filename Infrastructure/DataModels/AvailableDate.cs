using System;
using System.Collections.Generic;

namespace Infrastructure.DataModels
{
    public partial class AvailableDate
    {
        public int LecturerId { get; set; }
        public DateTime? Date { get; set; }
        public int SlotId { get; set; }

        public virtual User Lecturer { get; set; } = null!;
        public virtual StudySlot Slot { get; set; } = null!;
    }
}
