using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace LaywApplication.Controllers.APIUtils
{
    public static class Utils   
    {
        private static readonly HttpClient client = new HttpClient();

        public static JObject Get(string uri)
        {
            using (var client = new WebClient())
                return JObject.Parse(client.DownloadString(uri));//TODO gestire codice di errore
        }

        public async static Task<JObject> GetAsync(string uri)
        {
            using (var client = new WebClient())
                return JObject.Parse(await client.DownloadStringTaskAsync(uri));
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
}
