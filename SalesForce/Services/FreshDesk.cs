using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Main.Services
{
    public class FreshDesk
    {
        private const string fdDomain = "westcongrplatam";
        private const string _APIKey = "8dj562rj2kfSow2KhKV";
        private const string path = "/api/v2/tickets";
        private const string _Url = "https://" + fdDomain + ".freshdesk.com" + path;

        public static void GenerateTicket(string message, string exception)
        {
            // Basic auth:
            string login = _APIKey + ":X"; // It could be your username:password also.
            string credentials = credentials = Convert.ToBase64String(Encoding.Default.GetBytes(login));


            // Dados do ticket
            string email = "andre.vellinha@westcon.com";
            string title = "Rabbit MQ Exception";
            //string description = "Message: " + message + ".";
            //description += " --- Exception: " + exception;
            string description = "";
            //string json = "{\"status\": 2, \"priority\": 2, \"email\":\"{0}\",\"subject\":\"{1}\",\"description\":\"{2}\"}";
            //json = string.Format(json, email, title, description);
            string json = "{\"status\": 2, \"priority\": 2, \"source\": 1, \"email\":\"" + email + "\",\"subject\":\"" + title + "\",\"description\":\"" + description + "\"}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_Url);
            request.ContentType = "application/json";
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            request.ContentLength = byteArray.Length;
            request.Headers["Authorization"] = "Basic " + credentials;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            request.GetResponse();
            
        }
    }
}
