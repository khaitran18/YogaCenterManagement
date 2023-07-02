using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models.Response
{
    public class BaseResponse<TResponse>
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public TResponse Result { get; set; }
        public Exception Exception { get; set; }

        public BaseResponse()
        {

        }

        public BaseResponse(bool error, string message, Exception exception)
        {
            Error = error;
            Message = message;
            Exception = exception;
        }
    }
}
