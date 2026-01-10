using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Exceptions
{
    public sealed class ErrorValidationException : ApiException
    {
       public IEnumerable<string> Errors { get; }
        public ErrorValidationException(IEnumerable<string>? errors = null):base("Validation Failed", 400)
        {
            Errors = errors ?? Enumerable.Empty<string>();
            
            
        }
    }
}
