using HtmlAgilityPack;
using Lingsearcher.Models.API;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Lingsearcher.Services
{
    public class ProductAPIService
    {
        private string _store;
        private string _productId;

        public ProductAPIService(string store, string productId)
        {
            _store = store;
            _productId = productId;
        }
        public string GetHtmlProductPage(string store = "", string productId = "")
        {
            if(!String.IsNullOrEmpty(store) && !String.IsNullOrEmpty(productId))
            {
                _store = store;
                _productId = productId;
            }

            Dictionary<string, string> dictStore = new Dictionary<string, string>();
            List<string> listStores = new List<string>() { "aliexpress", "dealextreme" };

            if (!listStores.Contains(_store.ToLower()))
            {
                return String.Empty;
            }

            dictStore = (ConfigurationManager.GetSection($"StoreSettings/{_store}") as System.Collections.Hashtable)
                .Cast<System.Collections.DictionaryEntry>()
                .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());

            string url = $"{dictStore["url"]}{_productId}.html";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string data = String.Empty;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            return data;
        }
        public Product GetProductInfo(Dictionary<string, string> dictStore, string htmlProductPage)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlProductPage);

            /*
             * Regra que verifica:
             * Se o produto tem preço minimo
             * Caso nao tenha verifica se tem preco minimo em promocao
             * Caso não tenha verifica se tem preco unico em promoção
             * Caso não tenha indica que não foi encontrado
            */

            string minPrice =
                doc.DocumentNode.SelectNodes(dictStore["minPrice"]) != null
                ? doc.DocumentNode.SelectSingleNode(dictStore["minPrice"]).InnerText
                : doc.DocumentNode.SelectNodes(dictStore["minPricePromotion"]) != null
                ? doc.DocumentNode.SelectSingleNode(dictStore["minPricePromotion"]).InnerText
                : doc.DocumentNode.SelectNodes(dictStore["uniquePricePromotion"]) != null
                ? doc.DocumentNode.SelectSingleNode(dictStore["uniquePricePromotion"]).InnerText
                : doc.DocumentNode.SelectNodes(dictStore["uniquePrice"]) != null
                ? doc.DocumentNode.SelectSingleNode(dictStore["uniquePrice"]).InnerText
                : "Não encontrado";

            string maxPrice =
                doc.DocumentNode.SelectNodes(dictStore["maxPrice"]) != null
                ? doc.DocumentNode.SelectSingleNode(dictStore["maxPrice"]).InnerText
                : doc.DocumentNode.SelectNodes(dictStore["maxPricePromotion"]) != null
                ? doc.DocumentNode.SelectSingleNode(dictStore["maxPricePromotion"]).InnerText
                : String.Empty;

            string currency =
                doc.DocumentNode.SelectNodes(dictStore["currency"]) != null
                ? doc.DocumentNode.SelectSingleNode(dictStore["currency"]).InnerText
                : doc.DocumentNode.SelectSingleNode(dictStore["currencyPromotion"]).InnerText;

            Product product = new Product
            {
                Id = _productId,
                Store = _store,
                FullName = doc.DocumentNode.SelectSingleNode(dictStore["productName"]).InnerText,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Currency = currency.Trim()
                //ImageUrl = doc.DocumentNode.SelectSingleNode(dictStore["imageUrl"]).GetAttributeValue("src", null)
            };

            product.PriceRange = $"{product.MinPrice} - {product.MaxPrice}";

            return product;
        }
    }
}