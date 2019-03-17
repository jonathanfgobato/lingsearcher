using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lingsearcher.Entity
{
    public class Address : Domain
    {
        public string Street { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Neighbourhood { get; set; }

    }
}

