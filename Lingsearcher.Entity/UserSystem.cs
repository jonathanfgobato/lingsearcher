using Lingseacher.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingsearcher.Entity
{
    public class UserSystem: Domain
    {
        public UserApplication UserApplication { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public string UserApplicationId { get; set; }
        public short FgActive { get; set; }
    }
}
