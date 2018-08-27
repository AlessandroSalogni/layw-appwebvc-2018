using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers.PatientController
{
    public class WeightController : BasePatientController
    {
        public WeightController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> parameters) : base(IPconfig, parameters) {}

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<IActionResult> Read(int id)
        {
            JObject obj = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/weights?" + ParametersConfig.Date + "=23-06-2018"); //Request.Query["date"] //period
            JObject weights = (JObject)obj.GetValue("weights");
            return Json(weights);
        }
    }
}