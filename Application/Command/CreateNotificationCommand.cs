using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class CreateNotificationCommand : IRequest<Task>
    {
        public string? token { get; set; }
        public int scheduleid { get; set; }
        public string content { get; set; }
    }
}
