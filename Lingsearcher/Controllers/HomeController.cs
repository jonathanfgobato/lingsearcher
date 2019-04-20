using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Lingsearcher.DAL;
using Lingsearcher.Entity;
using Lingsearcher.Models.API;
using Lingsearcher.ViewModels;

namespace Lingsearcher.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string strFilter)
        {
            List<SearchProductsViewModel> model = new List<SearchProductsViewModel>();
            List<Product> products = (List<Product>)new BaseDAO<Product>().GetAll();

            foreach (var item in products)
            {
                item.Brand = new BaseDAO<Brand>().GetById(item.BrandId);
                item.Category = new BaseDAO<Category>().GetById(item.CategoryId);
            }

            strFilter = strFilter.ToLower();
            var productsSearch = from p in products
                                 where p.Name.ToLower().Contains(strFilter)
                                 || p.Brand.Name.ToLower().Contains(strFilter)
                                 || p.Category.Name.ToLower().Contains(strFilter)
                                 select p;

            foreach (var item in productsSearch)
            {
                List<ProductStore> productStore = (List<ProductStore>)new ProductStoreDAO().GetByProductId(item.Id);

                //Preencher lista de de SearchProductsViewModel com informacoes obtidas da lista productsSearch
                SearchProductsViewModel search = new SearchProductsViewModel
                {
                    IdProduct = item.Id,
                    NameProduct = item.Name,
                    NameBrand = item.Brand.Name,
                    ImageSrc = item.ImageSrc,
                    NameCategory = item.Category.Name,
                    ProductsAPI = new List<ProductAPI>()
                };

                foreach (var itemProductStore in productStore)
                {
                    itemProductStore.Store = new BaseDAO<Store>().GetById(itemProductStore.StoreId);

                    ProductAPI productAPI = new ProductAPI
                    {
                        MinPrice = itemProductStore.LastMinPrice,
                        MaxPrice = itemProductStore.LastMaxPrice,
                        Currency = itemProductStore.Currency,
                        ProductUrl = $"{itemProductStore.Store.UrlProduct}{itemProductStore.ProductStoreId}.html",
                        PriceRange = $"{itemProductStore.LastMinPrice}-{itemProductStore.LastMaxPrice}",
                        Store = itemProductStore.Store.Name
                    };

                    search.ProductsAPI.Add(productAPI);
                }

                string minPrice = search.ProductsAPI.Where(c => c.MinPrice > 0).Min(c => c.MinPrice).ToString();
                string maxPrice = search.ProductsAPI.Where(c => c.MaxPrice > 0).Max(c => c.MaxPrice).ToString();

                search.PriceRange = $"{minPrice}-{maxPrice}";
                model.Add(search);
            }

            //Retornar para a view que gera a visualização de produtos
            return View(@"../Product/ListProductsSearch", model);
        }
    }
}