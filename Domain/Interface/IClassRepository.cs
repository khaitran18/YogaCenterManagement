using Application.Interfaces;
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
        public Task<string> GetClassNotificationByClassIdAndSlotId(int classId, int slotId);
        public Task<bool> CheckSlotInClass(int classId, int slotId);
    }
}
