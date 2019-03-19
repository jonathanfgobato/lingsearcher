using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingsearcher.Entity
{
    public class Product : Domain
    {
        public Product() { }
        public Product(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string ImageSrc { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public List<ProductStore> ProductStores { get;set; }
    }
}
