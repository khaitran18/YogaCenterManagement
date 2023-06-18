using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ScheduleModel
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public int? SlotId { get; set; }
        public DateTime Date { get; set; }
        public string? DailyNote { get; set; }
    }
}
