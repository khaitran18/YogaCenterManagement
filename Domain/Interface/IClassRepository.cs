﻿using Application.Interfaces;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IClassRepository : IBaseRepository<ClassModel>
    {
        public Task<ClassModel> CreateClassSchedule(string name, double price, int capacity, DateTime startDate, DateTime endDate, List<int> dateIds);
        public Task<ClassModel> AssignLecturer(int classId, int lecturerId);
        public Task<string> GetClassNotificationByClassIdAndSlotId(int classId, int slotId);
        public Task<bool> CheckSlotInClass(int classId, int slotId);
        public Task<bool> CheckLecturerAuthority(int scheduleid, int userId);
        public Task<ClassModel> GetClassById(int classId);
    }
}
