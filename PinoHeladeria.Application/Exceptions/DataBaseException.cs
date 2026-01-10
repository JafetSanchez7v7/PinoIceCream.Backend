using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Exceptions
{
    public sealed class DataBaseException : ApiException
    {
        public DataBaseException(string message)
            : base(message, 500)
        {
            
        }
    }
}
