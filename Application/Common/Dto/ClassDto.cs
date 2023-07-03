using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dto
{
    public class ClassDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public int ClassCapacity { get; set; }
        public int? LecturerId { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public UserDto? Lecturer { get; set; }
        public List<UserDto>? Students { get; set; }
        public List<ScheduleDto>? Schedules { get; set; }
    }
}
