using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Exceptions
{
    public sealed class ConflictException : ApiException
    {
        public ConflictException(string message)
            : base(message, 409)
        {
        }
    }
  
}
