using Lingseacher.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingsearcher.DAL
{
    public class AddressDao : BaseDAO<Address>
    {
        public AddressDao()
        {
            Name = typeof(Address).Name;
        }

        public Address GetAddressByUserSystemId(long id)
        {
            return Query($"Spr_Buscar_{Name}_PorUserSystemId", new { UserSystemId = id }).SingleOrDefault();
        }
    }
}
