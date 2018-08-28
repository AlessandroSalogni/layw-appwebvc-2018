using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace LaywApplication.Controllers.PatientController
{
    public class GoalStepsDailyController : BasePatientController
    {
        private static readonly object Empty = new { };
        public GoalStepsDailyController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> parameters) : base(IPconfig, parameters)
        {}

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<Models.GoalsStepsDaily> Read(int id)
        {
            JObject obj = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/goals-steps-daily/current"); //Request.Query["date"] //period
            return ((JObject)obj.GetValue("goals-steps-daily")).GetObject<Models.GoalsStepsDaily>();
        }

        [HttpPost("~/dashboard/patients/{id}/[controller]/update")]
        public async Task<object> Update(int id, [FromBody]Models.GoalsStepsDaily item)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
            
            JObject jsonStepsObj = JObject.Parse(JsonConvert.SerializeObject(item, serializerSettings));

            var jsonSteps = new JObject
            {
                { "goals-steps", jsonStepsObj }
            };

            await APIUtils.PostAsync(IPconfig.GetTotalUrlUser() + id + "/goals-steps?period=daily", jsonSteps.ToString());

            return Empty;
        }
    }
}