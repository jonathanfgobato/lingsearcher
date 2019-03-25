using Lingsearcher.DAL;
using Lingsearcher.Entity;
using Lingsearcher.Models.API;
using Lingsearcher.Services;
using Lingsearcher.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            List<ProductAPI> productAPIs = new List<ProductAPI>();
            var stores = new BaseDAO<Store>().GetAll();

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

                foreach (var itemProductStore in productStore)
                {
                    itemProductStore.Store = new BaseDAO<Store>().GetById(itemProductStore.StoreId);
                }

                foreach (var itemProductStore in productStore)
                {
                    ProductAPIService productService = new ProductAPIService(itemProductStore.Store.Name, itemProductStore.ProductStoreId, item.Id);

                    string htmlProductPage = productService.GetHtmlProductPage(stores);

                    //Caso não venha vazio, adiciona a lista
                    if (!string.IsNullOrEmpty(htmlProductPage))
                    {
                        productAPIs.Add(productService.GetProductInfo(stores, htmlProductPage));
                    }
                }

                SearchProductsViewModel search = new SearchProductsViewModel
                {
                    IdProduct = item.Id,
                    NameProduct = item.Name,
                    NameBrand = item.Brand.Name,
                    ImageSrc =  item.ImageSrc,
                    NameCategory = item.Category.Name
                };

                string minPrice = productAPIs.Where(c => c.MinPrice > 0 && c.ProductId == item.Id).Min(c => c.MinPrice).ToString();
                string maxPrice = productAPIs.Where(c => c.MaxPrice > 0 && c.ProductId == item.Id).Max(c => c.MaxPrice).ToString();

                search.ProductsAPI = productAPIs.Where(p => p.ProductId == item.Id).ToList();
                search.PriceRange = $"{minPrice}-{maxPrice}";
                model.Add(search);
            }

            //Retornar para a view que gera a visualização de produtos
            return View(@"../Product/ListProductsSearch", model);
        }
    }
}