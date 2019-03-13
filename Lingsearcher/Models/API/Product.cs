using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lingsearcher.Models.API
{
    public class Product
    {
        public string Id { get; set; }
        public string Store { get; set; }
        public string FullName { get; set; }
        public string MinPrice { get; set; }
    }
}