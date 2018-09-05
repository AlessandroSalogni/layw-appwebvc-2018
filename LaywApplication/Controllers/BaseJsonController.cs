using LaywApplication.Configuration;
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
    }
}