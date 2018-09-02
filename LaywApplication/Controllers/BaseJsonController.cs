using LaywApplication.Configuration;

namespace LaywApplication.Controllers
{
    public abstract class BaseJsonController : BaseController
    {
        protected readonly QueryParams QueryParamsConfig;
        protected readonly AdditionalPath AdditionalPathConfig;
        protected readonly JsonData JsonDataConfig;

        public BaseJsonController(ServerIP IPConfig, JsonStructure jsonStructureConfig, JsonData jsonDataConfig) 
            : base(IPConfig)
        {
            QueryParamsConfig = jsonStructureConfig.QueryParams;
            AdditionalPathConfig = jsonStructureConfig.AdditionalPath;
            JsonDataConfig = jsonDataConfig;
        }
    }
}