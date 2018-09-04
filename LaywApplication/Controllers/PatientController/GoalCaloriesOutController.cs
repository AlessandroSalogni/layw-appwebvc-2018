using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers.PatientController
{
    public class GoalCaloriesOutController : BaseJsonController
    {
        public GoalCaloriesOutController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.GoalsCaloriesOut) { }
        
        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<Models.GoalsCaloriesOut> Read(int id, string date)
        {
            JObject goalCaloriesJson = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + id +
                JsonDataConfig.Url + EndUrlDate(Request, date));
            return ((JObject)goalCaloriesJson[JsonDataConfig.Root]).GetObject<Models.GoalsCaloriesOut>();
        }

        [HttpPost("~/dashboard/patients/{id}/[controller]/update")]
        public async Task<object> Update(int id, [FromBody]Models.GoalsCaloriesOut item)
        {
            var goalCaloriesJson = new JObject
            {
                { JsonDataConfig.Root, JObject.Parse(JsonConvert.SerializeObject(item, serializerSettings)) }
            };

            await APIUtils.PostAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url, goalCaloriesJson.ToString());
            return Empty;
        }
    }
}