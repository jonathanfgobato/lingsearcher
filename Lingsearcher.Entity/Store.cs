using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingsearcher.Entity
{
    public class Store : Domain
    {
        public int ProductPathId { get; set; }
        public string Name { get; set; }
        public string UrlStore { get; set; }
        public string UrlProduct { get; set; }
        public ProductPath ProductPath { get; set; }
    }
}
