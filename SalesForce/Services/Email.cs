using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Main.Services
{
    public class Email
    {
        public static void SendEmail(string message, string exception)
        {
            MailMessage mail = new MailMessage("andre.vellinha@westcon.com", "hugo.souza@westcon.com");
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("raphaelcmacedo@gmail.com", "013579Ra0286");
            
            mail.Subject = "Rabbit MQ Exception.";
            

            string body = "Message: " + message + "\n\n\n";
            body += "Exception: " + exception + "\n";
            mail.Body = body;
            client.Send(mail);
        }
    }
}
