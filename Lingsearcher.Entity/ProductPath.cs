using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingsearcher.Entity
{
    public class ProductPath : Domain
    {
        public string FullName { get; set; }
        public string Currency { get; set; }
        public string CurrencyPromotion { get; set; }
        public string MinPrice { get; set; }
        public string MinPricePromotion { get; set; }
        public string MaxPrice { get; set; }
        public string MaxPricePromotion { get; set; }
        public string UniquePrice { get; set; }
        public string UniquePricePromotion { get; set; }
        public string UrlImage { get; set; }
    }
}
