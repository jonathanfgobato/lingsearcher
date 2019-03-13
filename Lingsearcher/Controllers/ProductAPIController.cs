using HtmlAgilityPack;
using Lingsearcher.Models.API;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
            Dictionary<string, string> dictStore = new Dictionary<string, string>();
            List<string> listStores = new List<string>() { "aliexpress", "dealextreme" };

            if (!listStores.Contains(store.ToLower()))
            {
                return new Product();
            }

            dictStore = (ConfigurationManager.GetSection($"StoreSettings/{store}") as System.Collections.Hashtable)
                .Cast<System.Collections.DictionaryEntry>()
                .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());

            string url = $"{dictStore["url"]}{productId}.html";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string data = String.Empty;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            Product product = new Product
            {
                Id = productId,
                Store = store,
                FullName = doc.DocumentNode.SelectSingleNode(dictStore["productName"]).InnerText,
                MinPrice = doc.DocumentNode.SelectSingleNode(dictStore["minPrice"]).InnerText
            };

            return product;

        }
    }
}
