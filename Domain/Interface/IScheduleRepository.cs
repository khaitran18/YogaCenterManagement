using Application.Interfaces;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IScheduleRepository : IBaseRepository<ScheduleModel>
    {
        public Task<ScheduleModel> CreateNotification(int id, string notification);
        public Task<StudySlotModel> CreateSlot(TimeSpan startTime, TimeSpan endTime, List<int> dateIds);
        public Task<IEnumerable<StudySlotModel>> GetAllStudySlot();
        public Task<IEnumerable<AvailableDateModel>> AddAvailableDate(int lecturerId, List<int> slotIds);
        public Task<IEnumerable<AvailableDateModel>> GetAvailableDatesBySlotId(int slotId);
        public Task<IEnumerable<AvailableDateModel>> GetAvailableDatesByLecturerId(int lecturerId);
        public Task<bool> DeleteStudySlot(int studySlotId);
        public Task<bool> UpdateStudySlot(StudySlotModel studySlot);
        Task<bool> ExistSchedule(int scheduleId);
        Task<string> GetNotification(int scheduleId);
        public Task<bool> ExistStudySlot(TimeSpan startTime, TimeSpan endTime, List<int> dateIds);
        Task<List<ScheduleModel>> GetScheduleByStartDateAndEndDateAndClassId(DateTime startDate, DateTime endDate, int classId);
    }
}
