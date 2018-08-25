using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class ActivitiesController : BasePatientController
    {
        public ActivitiesController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> jsonStructure) : base(IPconfig, jsonStructure) { }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<IActionResult> Read(int id)
        {
            JObject obj = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/activities?" + ParametersConfig.Date + "=14-06-2018");
            JArray activities = (JArray)obj.GetValue("activities");
            return Json(new { activities });
        }
    }
}