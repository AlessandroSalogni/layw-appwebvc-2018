using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Controllers.Services.PatientData.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers.Services.PatientData
{
    [Route("~/dashboard/patients/{id}/[controller]")]
    public class GoalCaloriesOutController : BaseJsonReadController<Models.GoalCaloriesOut>
    {
        public GoalCaloriesOutController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.GoalsCaloriesOut) { }

        [HttpPost("update")]
        public async Task<object> Update(int id, [FromBody]Models.GoalCaloriesOut item)
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