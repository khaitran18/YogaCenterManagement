using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ClassModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public double Price { get; set; }
        public int ClassCapacity { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? LecturerId { get; set; }
        public short? ClassStatus { get; set; }
        public UserModel? Lecturer { get; set; }
        public List<UserModel>? Students { get; set; }
        public List<ScheduleModel>? Schedules { get; set; }
    }
}
