using Lingsearcher.DAL;
using Lingsearcher.Entity;
using Lingsearcher.Models.API;
using Lingsearcher.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace Lingsearcher.Controllers
{
    [RoutePrefix("Api/ProductAPI")]
    public class ProductAPIController : ApiController
    {
        [AcceptVerbs("GET")]
        [HttpGet]
        public Product GetProductInfo(string store, string productId)
        {
            /*
            Dictionary<string, string> dictStore = new Dictionary<string, string>();

            dictStore = (ConfigurationManager.GetSection($"StoreSettings/{store}") as System.Collections.Hashtable)
                .Cast<System.Collections.DictionaryEntry>()
                .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());
            */

            var stores = new BaseDAO<Store>().GetAll();
            ProductAPIService productService = new ProductAPIService(store, productId);

            string htmlProductPage = productService.GetHtmlProductPage(stores);

            //Caso não retorne nenhum html retorna o produto vazio
            if (string.IsNullOrEmpty(htmlProductPage))
            {
                return new Product();
            }

            return productService.GetProductInfo(stores, htmlProductPage);
        }
    }
}
