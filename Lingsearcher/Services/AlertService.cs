using Lingsearcher.DAL;
using Lingsearcher.Entity;
using System.Collections.Generic;
using System.Linq;
using Lingsearcher.Models.API;
using System.Configuration;
using System.Net.Mail;
using Lingsearcher.Utils;

namespace Lingsearcher.Services
{
    public class AlertService
    {
        private readonly string Email = ConfigurationManager.AppSettings["MailService:email_from"];

        public void VerifyAlerts()
        {
            //Ler todas as alertas para uma lista
            List<Alert> alerts = new BaseDAO<Alert>().GetAll().ToList();

            //Fazer iteracao de alertas
            foreach (var item in alerts)
            {
                //Caso tenha atingido o maximo de notificacoes pula para a proxima alerta
                if(item.MaxNumberNotifications == item.NumberNotificationsSend)
                {
                    continue;
                }

                item.Product = new BaseDAO<Product>().GetById(item.ProductId);

                //Preencher informacoes de usuario
                var userApplication = new UserApplicationDAO().GetEmailByUserSystemId(item.UserSystemId);

                //Buscar informacoes de produto
                List<ProductStore> productStore = (List<ProductStore>)new ProductStoreDAO().GetByProductId(item.ProductId);
                List<ProductAPI> productsAPI = new List<ProductAPI>();
                ProductAPI productAPI = null;

                foreach (var itemProductStore in productStore)
                {
                    ProductAPI itemProduct = new ProductAPI();
                    itemProductStore.Store = new BaseDAO<Store>().GetById(itemProductStore.StoreId);

                    itemProduct.MinPrice = itemProductStore.LastMinPrice;
                    itemProduct.MaxPrice = itemProductStore.LastMaxPrice;
                    itemProduct.Currency = itemProductStore.Currency;
                    itemProduct.ProductUrl = $"{itemProductStore.Store.UrlProduct}{itemProductStore.ProductStoreId}.html";
                    itemProduct.PriceRange = $"{itemProductStore.LastMinPrice}-{itemProductStore.LastMaxPrice}";
                    productsAPI.Add(itemProduct);
                }

                //Seleciona a loja com o menor valor de produto
                productAPI = productsAPI.OrderBy(p => p.MinPrice).First();

                //Verificar se preco do produto esta abaixo ou igual a alerta
                if (productAPI.MinPrice <= item.MinPrice)
                {
                    //Enviar email
                    var messageEmail = new MailMessage();
                    messageEmail.From = new MailAddress(Email);
                    messageEmail.Subject = $"Alert de preco - {item.Product.Name}";
                    messageEmail.To.Add(new MailAddress(userApplication.Email));
                    messageEmail.Body = $"ATENÇÃO O PRODUTO QUE VOCE DESEJA ESTA EM PROMOCAO - " +
                        $"{item.Product.Name}" +
                        $"Price {productAPI.Currency}{productAPI.MinPrice}" +
                        $"Link para a loja {productAPI.ProductUrl}";

                    MailHandler mailHandler = new MailHandler();
                    mailHandler.SendEmail(messageEmail);

                    //Atualizar quantidade de notificacoes na tabela Alert
                    item.NumberNotificationsSend++;
                    new AlertDAO().UpdateAlertNotification(item);
                }
            }
        }

    }
}