using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers.PatientController
{
    public class ActivitySummaryController : BasePatientController
    {
        public ActivitySummaryController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> parameters) : base(IPconfig, parameters) {}

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<IActionResult> GetSummaries(int id, string date, string period)
        {
            string periodParam = Request?.Query["period"] ?? period;
            string dateParam = Request?.Query["date"] ?? date;
            //todo tirare eccezione se uno dei due o tutti e due null?

            JObject obj = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/activity-summary?" + ParametersConfig.Date + "=" + dateParam + "&" + ParametersConfig.Period + "=" + periodParam); //Request.Query["date"] //period
            JArray activities = (JArray)obj.GetValue("activity-summary");
            return Json(activities);
        }
    }
}