using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers.PatientController
{
    public class ActivityController : BaseJsonController
    {
        public ActivityController(ServerIP IPConfig, JsonStructure jsonStructureConfig)
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Activities) { }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<List<Activity>> Read(int id, string date)
        {
            string dateParam = Request?.Query[QueryParamsConfig.Date] ?? date;
            //todo tirare eccezione se uno dei due o tutti e due null?

            JObject activityListJson = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url +
                "?" + QueryParamsConfig.Date + "=14-06-2018"/* + dateParam*/); //todo sistemare data
            return ((JArray)activityListJson[JsonDataConfig.Root]).GetList<Activity>();
        }
    }
}