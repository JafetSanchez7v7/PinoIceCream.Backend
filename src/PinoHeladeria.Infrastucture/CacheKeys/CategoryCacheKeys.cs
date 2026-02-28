using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.CacheKeys
{
    public static class CategoryCacheKeys
    {
        // Estas llaves son las llaves para crear los strings de memoria para el hashmap de IMemoryCache
        public const string CategoryList = "Categories_List";
        public const string CategoryByIdKey = "Category_Id_";
        public const string CategoryByNameKey = "Category_Name_";
        public const string ActiveCategories = "Active_List";
    }
}
