using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace LaywApplication.Controllers.PatientController
{
    public class GoalsWeightController : BasePatientController
    {
        private static readonly object Empty = new { };
        private IOptions<ServerIP> IPConfig;
        private IOptions<JsonStructure> Parameters;

        public GoalsWeightController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> parameters) : base(IPconfig, parameters)
        {
            IPConfig = IPconfig;
            Parameters = parameters;
        }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<Models.GoalsWeight> Read(int id)
        {
            JObject obj = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/goals-weight/current"); //Request.Query["date"] //period
            return ((JObject)obj.GetValue("goals-weights")).GetObject<Models.GoalsWeight>();
        }

        [HttpPost("~/dashboard/patients/{id}/[controller]/update")]
        public async Task<object> Update(int id, [FromBody]Models.GoalsWeight item)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "dd-MM-yyyy"
            };

            item.StartDate = DateTime.Now;

            JsonResult resWeight = (JsonResult)await new WeightController(IPConfig, Parameters).Read(id, DateTime.Now.ToShortDateString());
            Weights weightToday = JObject.Parse(resWeight.Value.ToString()).GetObject<Weights>();

            item.StartWeight = weightToday.Weight;

            JObject jsonStepsObj = JObject.Parse(JsonConvert.SerializeObject(item, serializerSettings));

            var jsonSteps = new JObject
            {
                { "goals-weights", jsonStepsObj }
            };

            await APIUtils.PostAsync(IPconfig.GetTotalUrlUser() + id + "/goals-weight", jsonSteps.ToString());

            return Empty;
        }
    }
}