﻿using HtmlAgilityPack;
using Lingsearcher.DAL;
using Lingsearcher.Entity;
using Lingsearcher.Models.API;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace Lingsearcher.Services
{
    public class ProductAPIService
    {
        private string _store;
        private string _productStoreId;
        private int _productId;
        private string _productUrl;

        public ProductAPIService(string store, string productStoreId)
        {
            _store = store;
            _productStoreId = productStoreId;
        }

        public ProductAPIService(string store, string productStoreId, int productId)
        {
            _store = store;
            _productStoreId = productStoreId;
            _productId = productId;
        }
        public string GetHtmlProductPage(IEnumerable<Store> stores, string store = "", string productId = "")
        {
            Store storeObj = null;

            if(!String.IsNullOrEmpty(store) && !String.IsNullOrEmpty(productId))
            {
                _store = store;
                _productStoreId = productId;
            }

            storeObj = (from item in stores
                        where item.Name.ToLower() == _store.ToLower()
                        select item).SingleOrDefault();

            //Fazer a verificação de nome de loja nao encontrada
            if(storeObj is null)
                return String.Empty;

            string url = $"{storeObj.UrlProduct}{_productStoreId}.html";
            _productUrl = url;

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
        public ProductAPI GetProductInfo(IEnumerable<Store> stores, string htmlProductPage)
        {
            Store storeObj = null;

            storeObj =
                (from item in stores
                 where item.Name.ToLower() == _store.ToLower()
                 select item).SingleOrDefault();

            ProductPath productPath = new BaseDAO<ProductPath>().GetById(storeObj.ProductPathId);

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
                doc.DocumentNode.SelectNodes($"{productPath.MinPrice}text()") != null
                ? doc.DocumentNode.SelectSingleNode($"{productPath.MinPrice}text()").InnerText
                : doc.DocumentNode.SelectNodes($"{productPath.MinPricePromotion}text()") != null
                ? doc.DocumentNode.SelectSingleNode($"{productPath.MinPricePromotion}text()").InnerText
                : doc.DocumentNode.SelectNodes($"{productPath.UniquePricePromotion}text()") != null
                ? doc.DocumentNode.SelectSingleNode($"{productPath.UniquePricePromotion}text()").InnerText
                : doc.DocumentNode.SelectNodes($"{productPath.UniquePrice}text()") != null
                ? doc.DocumentNode.SelectSingleNode($"{productPath.UniquePrice}text()").InnerText
                : "Não encontrado";

            string maxPrice =
                doc.DocumentNode.SelectNodes($"{productPath.MaxPrice}text()") != null
                ? doc.DocumentNode.SelectSingleNode($"{productPath.MaxPrice}text()").InnerText
                : doc.DocumentNode.SelectNodes($"{productPath.MaxPricePromotion}text()") != null
                ? doc.DocumentNode.SelectSingleNode($"{productPath.MaxPricePromotion}text()").InnerText
                : String.Empty;

            string currency =
                doc.DocumentNode.SelectNodes($"{productPath.Currency}text()") != null
                ? doc.DocumentNode.SelectSingleNode($"{productPath.Currency}text()").InnerText
                : doc.DocumentNode.SelectSingleNode($"{productPath.CurrencyPromotion}text()").InnerText;

            //minPrice = minPrice.Remove(minPrice.Length - 3, 1).Insert(minPrice.Length - 3, ",");
            //maxPrice = minPrice.Remove(maxPrice.Length - 3, 1).Insert(maxPrice.Length - 3, ",");
            double minPriceNumber, maxPriceNumber;

            //minPriceNumber = Convert.ToDouble(minPrice);
            //maxPriceNumber = Convert.ToDouble(maxPrice);
            //var minPriceNumber = double.Parse(minPrice, CultureInfo.InvariantCulture);

            CultureInfo info = new CultureInfo("pt-BR");
            info.NumberFormat.NumberDecimalSeparator = ",";
            Thread.CurrentThread.CurrentCulture = info;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            minPriceNumber = Convert.ToDouble(minPrice.Remove(minPrice.Length - 3, 1).Insert(minPrice.Length - 3, ","));

            if (!String.IsNullOrEmpty(maxPrice))
            {
                maxPriceNumber = Convert.ToDouble(maxPrice.Remove(maxPrice.Length - 3, 1).Insert(maxPrice.Length - 3, ","));
            }
            else
            {
                maxPriceNumber = minPriceNumber;
            }
            ProductAPI product = new ProductAPI
            {
                Id = _productStoreId,
                Store = _store,
                FullName = doc.DocumentNode.SelectSingleNode($"{productPath.FullName}text()").InnerText,
                MinPrice = minPriceNumber,
                MaxPrice = maxPriceNumber,
                Currency = currency.Trim(),
                ProductId = _productId,
                ProductUrl = _productUrl
                //ImageUrl = doc.DocumentNode.SelectSingleNode(dictStore["imageUrl"]).GetAttributeValue("src", null)
            };

            product.PriceRange = $"{product.MinPrice} - {product.MaxPrice}";

            return product;
        }
    }
}