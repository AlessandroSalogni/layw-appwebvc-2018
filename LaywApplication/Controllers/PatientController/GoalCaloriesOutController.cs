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
    public class GoalCaloriesOutController : BasePatientController
    {
        private static readonly object Empty = new { };

        public GoalCaloriesOutController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> parameters) : base(IPconfig, parameters)
        {
        }
        
        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<IActionResult> GetGoal(int id)
        {
            JObject obj = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/goals-calories-out/current"); //Request.Query["date"] //period
            JObject goal = (JObject)obj.GetValue("goals-calories-out");
            return Json(goal);
        }

        [HttpPost("~/dashboard/patients/{id}/[controller]/create")]
        public async Task<object> PostGoal(int id, [FromBody]Models.GoalsCaloriesOut item)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "dd-MM-yyyy"
            };

            item.Date = DateTime.Now;

            JObject jsonStepsObj = JObject.Parse(JsonConvert.SerializeObject(item, serializerSettings));

            var jsonSteps = new JObject
            {
                { "goals-calories-out", jsonStepsObj }
            };

            await APIUtils.PostAsync(IPconfig.GetTotalUrlUser() + id + "/goals-calories-out", jsonSteps.ToString());

            return Empty;
        }
    }
}