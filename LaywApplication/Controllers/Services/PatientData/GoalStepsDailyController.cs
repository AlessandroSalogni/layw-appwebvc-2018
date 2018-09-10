using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Services.PatientData.Abstract;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models.PatientData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers.Services.PatientData
{
    [Route("~/dashboard/patients/{id}/[controller]")]
    public class GoalStepsDailyController : BaseJsonReadController<GoalStepsDaily>
    {
        public GoalStepsDailyController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.GoalsStepsDaily) { }

        [HttpPost("update")]
        public async Task<object> Update(int id, [FromBody]GoalStepsDaily item)
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