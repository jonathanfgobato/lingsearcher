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
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string PriceRange { get; set; }
        public string Currency { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public string ProductUrl { get; set; }
    }
}