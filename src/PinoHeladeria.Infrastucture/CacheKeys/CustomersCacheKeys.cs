using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.CacheKeys
{
    public static class CustomersCacheKeys
    {
        public const string CustomerList = "customersList";
        public const string CustomerByIdKey = "customerById_";
        public const string CustomerByName = "customerByName_";
        public const string ActiveList = "activeCustomersList";
    }
}
