using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaywApplication.Controllers.PatientController
{
    public class BaseJsonReadController<TModel> : BaseJsonController where TModel : class
    {
        public BaseJsonReadController(ServerIP IPConfig, JsonStructure jsonStructureConfig, JsonData jsonDataConfig) :
            base(IPConfig, jsonStructureConfig, jsonDataConfig) { }

        [HttpGet]
        public async Task<TModel> Read(int id, string date)
        {
            JObject json = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url +
                EndUrl(Request, date));
            return (json == null) ? null : (json[JsonDataConfig.Root] as JObject)?.GetObject<TModel>();
        }

        [HttpGet("list")]
        public async Task<List<TModel>> Read(int id, string date, string period)
        {
            JObject listJson = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url +
                EndUrl(Request, date, period));

            if (listJson == null)
                return new List<TModel>();

            return (listJson[JsonDataConfig.Root] as JArray)?.GetList<TModel>() ??
                new List<TModel>
                {
                    (listJson[JsonDataConfig.Root] as JObject)?.GetObject<TModel>()
                };
        }

        private string EndUrl(HttpRequest request, string date)
        {
            string dateParam = Request?.Query[QueryParamsConfig.Date] ?? date;
            return (dateParam == null) ? AdditionalPathConfig.Current :
                "?" + QueryParamsConfig.Date + "=" + dateParam;
        }

        private string EndUrl(HttpRequest request, string date, string period)
        {
            string periodParam = Request?.Query[QueryParamsConfig.Period] ?? period;
            string dateParam = Request?.Query[QueryParamsConfig.Date] ?? date;

            if (periodParam == null || dateParam == null)
                throw new NullReferenceException();

            return "?" + QueryParamsConfig.Date + "=" + dateParam + "&" +
                QueryParamsConfig.Period + "=" + periodParam;
        }
    }
}
