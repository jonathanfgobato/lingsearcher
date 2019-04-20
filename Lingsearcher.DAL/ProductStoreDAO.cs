using Lingsearcher.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingsearcher.DAL
{
    public class ProductStoreDAO : BaseDAO<ProductStore>
    {
        public virtual IEnumerable<ProductStore> GetByProductId(int productId)
        {
            return Query($"Spr_Listar_{Name}_PorProductId", new { ProductId = productId });
        }
        public void UpdateInfoProducts(ProductStore product)
        {
            ExecuteProcedure($"Spr_AtualizarPrecos_ProductStore", new
            {
                product.ProductId,
                product.StoreId,
                product.LastMinPrice,
                product.LastMaxPrice,
                product.Currency
            });
        }
    }
}
