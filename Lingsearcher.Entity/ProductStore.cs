using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingsearcher.Entity
{
    public class ProductStore : Domain
    {
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public string ProductStoreId { get; set; }
        public Store Store { get; set; }
    }
}
