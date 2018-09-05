﻿using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
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
}
