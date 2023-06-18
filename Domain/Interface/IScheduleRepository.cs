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
    }
}
