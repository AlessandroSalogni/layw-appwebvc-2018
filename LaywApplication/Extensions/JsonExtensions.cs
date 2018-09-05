using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace LaywApplication.Controllers.Utils
{
    public static class JsonExtensions
    {
        private static readonly IsoDateTimeConverter DateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };

        public static T GetObject<T>(this JObject jsonObject)
        {
            return JsonConvert.DeserializeObject<T>(jsonObject.ToString(), DateTimeConverter);
        }

        public static List<T> GetList<T>(this JArray jsonArray)
        {
            List<T> list = new List<T>();
            foreach (JObject jsonObject in jsonArray)
                list.Add(JsonConvert.DeserializeObject<T>(jsonObject.ToString(), DateTimeConverter));

            return list;
        }
    }
}
