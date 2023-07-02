using AutoMapper;
using Domain.Interface;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ScheduleRepository : BaseRepository<ScheduleModel>, IScheduleRepository
    {
        private readonly YGCContext _context;
        private readonly IMapper _mapper;

        public ScheduleRepository(YGCContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ScheduleModel> CreateNotification(int id, string notification)
        {
            try
            {
                ScheduleModel model = new ScheduleModel();
                Schedule? schedule = _context.Schedules.FirstOrDefault(s => s.Id == id);
                if (schedule != null)
                {
                    schedule.DailyNote = notification;
                    //_context.Entry(schedule).State = EntityState.Modified;
                    //await UpdateAsync(_mapper.Map<ScheduleModel>(schedule));
                }
                model = _mapper.Map<ScheduleModel>(schedule);
                _context.SaveChanges();
                return await Task.FromResult(model);
            }
            catch
            {
                throw new Exception("Error in adding new daily note");
            }
        }

        #region Create a new StudySlot
        /// <summary>
        /// Create a new StudySlot
        /// </summary>
        /// <param name="startTime">Start time of a slot</param>
        /// <param name="endTime">End time of a slot</param>
        /// <param name="dateId">Id of DateOfWeeks</param>
        /// <returns>None</returns>
        /// <exception cref="Exception"></exception>
        public async Task<StudySlotModel> CreateSlot(TimeSpan startTime, TimeSpan endTime, List<int> dateIds)
        {
            StudySlotModel studySlotModel = new StudySlotModel();
            try
            {
                StudySlot studySlot = new StudySlot { StartTime = startTime, EndTime = endTime };
                foreach (var dayId in dateIds)
                {
                    var dateOfWeek = await _context.DateOfWeeks.FindAsync(dayId);
                    if (dateOfWeek != null)
                    {
                        studySlot.Days.Add(dateOfWeek);
                    }
                    else
                    {
                        throw new Exception("Date of Week does not exist");
                    }
                }
                studySlotModel = _mapper.Map<StudySlotModel>(studySlot);
                _context.StudySlots.Add(studySlot);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Error when creating study slot");
            }
            return studySlotModel;
        }
        #endregion

        #region Get all study slots
        public async Task<IEnumerable<StudySlotModel>> GetAllStudySlot()
        {
            var studySlotModels = new List<StudySlotModel>();
            try
            {
                var studySlots = await _context.StudySlots.Include(ss => ss.Days).ToListAsync();
                studySlotModels = _mapper.Map<List<StudySlotModel>>(studySlots);
            }
            catch (Exception)
            {

                throw new Exception("Error in Get all study slot");
            }
            return studySlotModels;
        }
        #endregion

        #region Delete study slot by slot id
        public async Task<bool> DeleteStudySlot(int studySlotId)
        {
            var check = false;
            try
            {
                var existStudySlot = await _context.StudySlots.FirstOrDefaultAsync(ss => ss.SlotId == studySlotId);
                if(existStudySlot != null)
                {
                    _context.StudySlots.Remove(existStudySlot);
                    await _context.SaveChangesAsync();
                    check = true;

                }
            }
            catch (Exception)
            {

                throw new Exception("Error in delete study slot");
            }
            return check;
        }
        #endregion

        #region Update Study Slot
        public async Task<bool> UpdateStudySlot(StudySlotModel studySlot)
        {
            var check = false;
            try
            {
                var studySlotData = _mapper.Map<StudySlot>(studySlot);
                _context.StudySlots.Update(studySlotData);
                await _context.SaveChangesAsync();
                check = true;
            }
            catch (Exception)
            {

                throw new Exception("Error in update study slot");
            }
            return check;
        }
        #endregion

        #region Add lecturer free slots
        /// <summary>
        /// Add lecturer free slots
        /// </summary>
        /// <param name="lectuterId">Id of available lecturer</param>
        /// <param name="slotIds">Id list of available slots</param>
        /// <returns>None</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<AvailableDateModel>> AddAvailableDate(int lectuterId, List<int> slotIds)
        {
            var availableDates = new List<AvailableDateModel>();
            try
            {
                foreach (var slotId in slotIds)
                {
                    var studySlot = await _context.StudySlots.FindAsync(slotId);
                    AvailableDate availableDate = new AvailableDate { LecturerId = lectuterId };
                    if (studySlot != null)
                    {
                        availableDate.SlotId = slotId;
                        _context.AvailableDates.Add(availableDate);
                        await _context.SaveChangesAsync();
                        availableDates.Add(_mapper.Map<AvailableDateModel>(availableDate));

                    }
                }
            }
            catch (Exception)
            {

                throw new Exception("Error when add available date");
            }
            return availableDates;
        }


        #endregion

        #region Get available dates by slot id
        /// <summary>
        /// Get available dates by slot id
        /// </summary>
        /// <param name="slotId">Slot id of the available dates</param>
        /// <returns>List of available date model</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<AvailableDateModel>> GetAvailableDatesBySlotId(int slotId)
        {
            var availableDateModels = new List<AvailableDateModel>()
;            try
            {
                var availableDates = await _context.AvailableDates.Where(ad => ad.SlotId == slotId).ToListAsync();
                availableDateModels = _mapper.Map<List<AvailableDateModel>>(availableDates);
            }
            catch (Exception)
            {

                throw new Exception("Error in getting available dates");
            }
            return availableDateModels;
        }
        #endregion

        #region Get available dates by lecturer id
        public async Task<IEnumerable<AvailableDateModel>> GetAvailableDatesByLecturerId(int lecturerId)
        {
            var availableDateModels = new List<AvailableDateModel>()
; try
            {
                var availableDates = await _context.AvailableDates.Where(ad => ad.LecturerId == lecturerId).ToListAsync();
                availableDateModels = _mapper.Map<List<AvailableDateModel>>(availableDates);
            }
            catch (Exception)
            {

                throw new Exception("Error in getting available dates");
            }
            return availableDateModels;
        }
        #endregion

        public async Task<bool> ExistSchedule(int scheduleId)
        {
            try
            {
                bool c = _context.Schedules.Any(s => s.Id == scheduleId);
                return await Task.FromResult(c);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> GetNotification(int scheduleId)
        {
            try
            {
                string result = _context.Schedules.FirstOrDefaultAsync(s => s.Id == scheduleId).Result.DailyNote;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
