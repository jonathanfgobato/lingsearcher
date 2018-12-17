using Lingseacher.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingsearcher.DAL
{
    public interface IDAO<T> where T : Domain
    {
        T Insert(T entity);
        void Update(T entity);
        int Delete(int id);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}
