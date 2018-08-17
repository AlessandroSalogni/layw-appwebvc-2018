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
        private const string dateFormat = "dd-MM-yyyy";

        public static List<T> Get<T>(string baseUrl, string url, string root) where T : class, new()
        {
            var client = new RestClient(baseUrl);
            var rq = new RestRequest(url, Method.GET);
            var rs = client.Execute(rq);

            JObject json = JObject.Parse(rs.Content);
            JArray jsonArray = (JArray)json.GetValue(root);

            List<T> list = new List<T>();
            foreach (JObject obj in jsonArray)
                list.Add(JsonConvert.DeserializeObject<T>(obj.ToString()));
            
            return list;
        }

        /*
        public static string Post(string baseUrl, string url, string json)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(url, Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddBody(json);

            var response = client.Execute(request);
            return response.Content;
        }
        */
        
        private static readonly HttpClient client = new HttpClient();

        public static string Get(string uri)
        {
            string result;
            using (var client = new WebClient())
                result = client.DownloadString(uri);//TODO gestire codice di errore
            return result;
        }
        //public async static Task<T> GetAwait(string uri)
        //{
        //    string result;
        //    using (var client = new WebClient())
        //        result = await client.DownloadString(uri);
        //    return result;
        //}

        public static string Post(string uri, string body)
        {
            string result;
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                result = client.UploadString(uri, "POST", body);
            }
            return result;
        }
        


    }
}
