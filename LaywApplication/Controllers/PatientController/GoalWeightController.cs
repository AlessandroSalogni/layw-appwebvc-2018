using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers.PatientController
{
    public class GoalWeightController : BaseJsonController
    {
        public GoalWeightController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.GoalsWeight) { }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<Models.GoalsWeight> Read(int id, string date)
        {
            JObject goalWeightJson = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + id +
                JsonDataConfig.Url + EndUrlDate(Request, date));
            return ((JObject)goalWeightJson[JsonDataConfig.Root]).GetObject<Models.GoalsWeight>();
        }

        [HttpPost("~/dashboard/patients/{id}/[controller]/update")]
        public async Task<object> Update(int id, [FromBody]Models.GoalsWeight item)
        {
            item.StartDate = DateTimeNow;
            item.StartWeight = (await new WeightController(IPConfig, JsonStructureConfig).
                Read(id, DateTimeNow.ToShortDateString())).Weight;

            var jsonGoalWeight = new JObject
            {
                { JsonDataConfig.Root , JObject.Parse(JsonConvert.SerializeObject(item, serializerSettings)) }
            };

            await APIUtils.PostAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url, jsonGoalWeight.ToString());
            return Empty;
        }
    }
}