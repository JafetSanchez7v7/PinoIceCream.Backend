using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.CacheKeys
{
    public static class  ProductCacheKeys
    {
        // Estas llaves son las llaves para crear los strings de memoria para el hashmap de IMemoryCache
        public const string ProductList = "Products_List";
        public const string ProductByIdKey = "Product_Id_";
        public const string ProductByNameKey = "Product_Name_";
        public const string ActiveProducts = "Active_List";
    }
}
