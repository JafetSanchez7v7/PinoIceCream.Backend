using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.CacheKeys
{
    public static class InventoryCacheKeys
    {
        public const string InventoryList = "InventoryList";
        public const string InventoryByIdKey = "InventoryById_";
        public const string InventoryByProductId = "InventoryByProductId_";
        public const string InventoryByProductName = "InventoryByProductName_";
        public const string InventoryByStockFilter = "InventoryByStockFilterList";
       

    }
}
