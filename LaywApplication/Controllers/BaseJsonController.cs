using LaywApplication.Configuration;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LaywApplication.Controllers
{
    public abstract class BaseJsonController : BaseController
    {
        protected static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "dd-MM-yyyy"
        };

        protected readonly JsonStructure JsonStructureConfig;
        protected readonly QueryParams QueryParamsConfig;
        protected readonly AdditionalPath AdditionalPathConfig;
        protected readonly JsonData JsonDataConfig;

        public BaseJsonController(ServerIP IPConfig, JsonStructure jsonStructureConfig, JsonData jsonDataConfig) 
            : base(IPConfig)
        {
            JsonStructureConfig = jsonStructureConfig;
            QueryParamsConfig = jsonStructureConfig.QueryParams;
            AdditionalPathConfig = jsonStructureConfig.AdditionalPath;
            JsonDataConfig = jsonDataConfig;
        }

        protected string EndUrlDate(HttpRequest request, string date)
        {
            string dateParam = Request?.Query[QueryParamsConfig.Date] ?? date;
            return (dateParam == null) ? AdditionalPathConfig.Current : 
                "?" + QueryParamsConfig.Date + "=" + dateParam;
        }
    
        protected string EndUrlDatePeriod(HttpRequest request, string date, string period)
        {
            string periodParam = Request?.Query[QueryParamsConfig.Period] ?? period;
            string dateParam = Request?.Query[QueryParamsConfig.Date] ?? date;

            return (dateParam == null) ? AdditionalPathConfig.Current : "?" + QueryParamsConfig.Date + "=" +
                dateParam + ((periodParam == null) ? "" : "&" + QueryParamsConfig.Period + "=" + periodParam);
        }
    }
}