using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Exceptions
{
    public abstract class ApiException : Exception
    {
        public int StatusCode { get; }
        protected ApiException(string message, int statusCode, Exception? innerEx = null ) 
            : base(message, innerEx)
        {
            StatusCode = statusCode;
        }

        
    }
}
