using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lingsearcher.Models.API
{
    public class ProductAPI
    {
        public string Id { get; set; }
        public string Store { get; set; }
        public string FullName { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string PriceRange { get; set; }
        public string Currency { get; set; }
        public string ImageUrl { get; set; }
    }
}