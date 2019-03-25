using Lingsearcher.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lingsearcher.ViewModels
{
    public class SearchProductsViewModel
    {
        public int IdProduct { get; set; }
        public string NameProduct { get; set; }
        public string ImageSrc { get; set; }
        public string NameCategory { get; set; }
        public string NameBrand { get; set; }
        public string PriceRange { get; set; }
        public List<ProductAPI> ProductsAPI { get; set; }
    }
}