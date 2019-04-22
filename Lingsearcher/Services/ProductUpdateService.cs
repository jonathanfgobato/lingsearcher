using Lingsearcher.DAL;
using Lingsearcher.Entity;
using Lingsearcher.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lingsearcher.Services
{
    public class ProductUpdateService
    {
        public void UpdatePrices()
        {
            List<ProductStore> productsStore = (List<ProductStore>)new ProductStoreDAO().GetAll();
            //List<ProductAPI> productAPIs = new List<ProductAPI>();
            var stores = new BaseDAO<Store>().GetAll();
            ProductAPI productApi = null;

            foreach (var itemProductStore in productsStore)
            {
                itemProductStore.Store = new BaseDAO<Store>().GetById(itemProductStore.StoreId);
            }

            //Varrer lista de de produtos e atualizar preco
            foreach (var itemProductStore in productsStore)
            {
                ProductAPIService productService = new ProductAPIService(itemProductStore.Store.Name, itemProductStore.ProductStoreId, itemProductStore.Id);

                string htmlProductPage = productService.GetHtmlProductPage(stores);

                //Caso não venha vazio, adiciona a lista
                if (!string.IsNullOrEmpty(htmlProductPage))
                {
                    //productAPIs.Add(productService.GetProductInfo(stores, htmlProductPage));
                    productApi = productService.GetProductInfo(stores, htmlProductPage);
                }

                //Adicionar precos e currency a lista de produtos
                itemProductStore.LastMinPrice = productApi.MinPrice;
                itemProductStore.LastMaxPrice = productApi.MaxPrice;
                itemProductStore.Currency = productApi.Currency;

                //Alteracao temporaria para todos os precos ficarem em R$
                if(itemProductStore.Store.Name.ToLower() == "dealextreme")
                {
                    itemProductStore.LastMinPrice *= 4;
                    itemProductStore.LastMaxPrice *= 4;
                    itemProductStore.Currency = "R$"; 
                }

                //Atualizar tabela de ProductStore
                new ProductStoreDAO().UpdateInfoProducts(itemProductStore);
            }
           
        }
    }
}