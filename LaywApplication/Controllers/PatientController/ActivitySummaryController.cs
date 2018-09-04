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
        public async Task<Models.ActivitySummary> Read(int id, string date)
        {
            JObject summaryJson = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url +
                EndUrlDate(Request, date)); 
            return ((JObject)summaryJson[JsonDataConfig.Root]).GetObject<Models.ActivitySummary>();
        }

        [HttpGet("~/dashboard/patients/{id}/[controller]/list")]
        public async Task<List<Models.ActivitySummary>> Read(int id, string date, string period)
        {
            JObject summaryListJson = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url +
                EndUrlDatePeriod(Request, date, period));
            return ((JArray)summaryListJson[JsonDataConfig.Root]).GetList<Models.ActivitySummary>();
        }
    }
}