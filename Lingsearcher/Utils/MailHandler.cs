using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Lingsearcher.Utils
{
    public class MailHandler
    {
        private readonly string Email = ConfigurationManager.AppSettings["MailService:email_from"];
        private readonly string Email_Password = /*ConfigurationManager.AppSettings["MailService:email_password"]*/ "jhn2391k";

        public void SendEmail(MailMessage message)
        {
            // SMTP - Simple Mail Transport Protocol

            using (var smtpClient = new SmtpClient())
            {
                // Conexão com a caixa de email da aplicação
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(Email, Email_Password);

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;

                smtpClient.Timeout = 20000;

                smtpClient.Send(message);
            }
        }

    }
}