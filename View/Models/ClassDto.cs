﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models
{
    public class ClassDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public int ClassCapacity { get; set; }
        public int? LecturerId { get; set; }
        public UserDto? Lecturer { get; set; }
        public List<UserDto> Students { get; set; }
        public List<ScheduleDto> Schedules { get; set; }
    }
}