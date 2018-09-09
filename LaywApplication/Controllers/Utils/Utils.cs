using LaywApplication.Configuration;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaywApplication.Controllers.Utils
{
    public static class APIUtils
    {
        private static IsoDateTimeConverter DateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };
        private static readonly HttpClient client = new HttpClient();

        public static JObject Get(string uri)
        {
            using (var client = new WebClient())
                try { return JObject.Parse(client.DownloadString(uri)); }
                catch (Exception) { return null; }
        }

        public async static Task<JObject> GetAsync(string uri)
        {
            using (var client = new WebClient())
                try { return JObject.Parse(await client.DownloadStringTaskAsync(uri)); }
                catch (Exception) { return null; }
        }

        public static JObject Post(string uri, string body)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                return JObject.Parse(client.UploadString(uri, "POST", body));
            }
        }

        public async static Task<JObject> PostAsync(string uri, string body)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                return JObject.Parse(await client.UploadStringTaskAsync(uri, "POST", body));
            }
        }
    }
    
    public class Emailer
    {
        private MailData MailData { get; set; }

        public Emailer(MailData mailData)
        {
            MailData = mailData;
        }

        public void SendEmail(string email, string message, string subject)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(MailData.SMTP);

                mail.From = new MailAddress(MailData.Email);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = message;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Port = MailData.Port;
                SmtpServer.Credentials = new NetworkCredential(MailData.Email, MailData.Password);
                SmtpServer.EnableSsl = MailData.EnableSSL;
                SmtpServer.Timeout = MailData.TimeOut;
                SmtpServer.Send(mail);
            }
            catch (Exception e) when (e is SmtpException || e is FormatException)
            {
                throw e;
            }
        }
    }

    public class PasswordUtils
    {
        public static string PasswordGenerator()
        {
            Random random = new Random();
            string chars = "abcdefghilmnopqrstuvz0123456789";
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string MD5Crypt(string value)
        {
            using (var md5 = MD5.Create())
            {
                return Encoding.Default.GetString(md5.ComputeHash(Encoding.ASCII.GetBytes(value)));
            }
        }
    }
}
