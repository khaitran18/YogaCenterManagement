using Application.Common;
using Application.Common.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class CreateClassCommand : IRequest<BaseResponse<ClassDto>>
    {
        public string? token { get; set; }
        public string ClassName { get; set; }
        public double Price { get; set; }
        public int ClassCapacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SelectedDayOfWeek { get; set; }
    }
}
