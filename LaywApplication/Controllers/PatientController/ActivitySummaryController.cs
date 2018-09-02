using System.Collections.Generic;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers.PatientController
{
    public class ActivitySummaryController : BaseJsonController
    {
        public ActivitySummaryController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.ActivitySummary) {}

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<List<Models.ActivitySummary>> Read(int id, string date, string period)
        {
            string periodParam = Request?.Query[QueryParamsConfig.Period] ?? period;
            string dateParam = Request?.Query[QueryParamsConfig.Date] ?? date;
            //todo tirare eccezione se uno dei due o tutti e due null?

            JObject summaryListJson = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url + "?" + 
                QueryParamsConfig.Date + "=" + dateParam + "&" + QueryParamsConfig.Period + "=" + periodParam); 
            return ((JArray)summaryListJson[JsonDataConfig.Root]).GetList<Models.ActivitySummary>();
        }
    }
}