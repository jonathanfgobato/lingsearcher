using Lingsearcher.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingsearcher.DAL
{
    public class UserSystemDao: BaseDAO<UserSystem>
    {
        public UserSystemDao()
        {
            Name = typeof(UserSystem).Name;
        }

        public UserSystem GetByUserApplicationId(string id)
        {
            return Query($"Spr_Buscar_{Name}_PorUserApplicationId", new { UserApplicationId = id }).SingleOrDefault();
        }
    }
}
