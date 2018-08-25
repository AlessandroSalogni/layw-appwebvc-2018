using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace LaywApplication.Controllers.Utils
{
    public static class JsonExtensions
    {
        public static List<T> GetList<T>(this JArray jsonArray)
        {
            List<T> list = new List<T>();
            foreach (JObject obj in jsonArray)
                list.Add(JsonConvert.DeserializeObject<T>(obj.ToString()));

            return list;
        }
    }
}
