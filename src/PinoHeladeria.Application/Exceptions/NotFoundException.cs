using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Exceptions
{
    public sealed class NotFoundException : ApiException
    {
        public NotFoundException(string message) 
            : base(message, 404)
        {
        }
    }
}
