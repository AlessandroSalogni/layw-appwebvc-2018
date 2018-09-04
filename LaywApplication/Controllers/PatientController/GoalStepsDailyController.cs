using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers.PatientController
{
    public class GoalStepsDailyController : BaseJsonController
    {
        public GoalStepsDailyController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.GoalsStepsDaily) { }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<Models.GoalStepsDaily> Read(int id, string date)
        {
            JObject goalStepsJson = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + id +
                JsonDataConfig.Url + EndUrlDate(Request, date));
            return ((JObject)goalStepsJson[JsonDataConfig.Root]).GetObject<Models.GoalStepsDaily>();
        }

        [HttpPost("~/dashboard/patients/{id}/[controller]/update")]
        public async Task<object> Update(int id, [FromBody]Models.GoalStepsDaily item)
        {
            var GoalsStepsDailyConfig = JsonDataConfig as GoalsStepsDaily;
            var goalStepsJson = new JObject
            {
                { GoalsStepsDailyConfig.RootUpdate, JObject.Parse(JsonConvert.SerializeObject(item, serializerSettings)) }
            };

            await APIUtils.PostAsync(IPConfig.GetTotalUrlUser() + id + GoalsStepsDailyConfig.UrlUpdate + "?"
                + QueryParamsConfig.Period + "=" + GoalsStepsDailyConfig.Period, goalStepsJson.ToString());
            return Empty;
        }
    }
}