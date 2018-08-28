using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.PatientController;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class PatientPersonalDataController : BasePatientController
    {
        private IOptions<ServerIP> IPConfig;
        private IOptions<JsonStructure> Parameters;

        public PatientPersonalDataController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> parameters) : base(IPconfig, parameters)
        {
            IPConfig = IPconfig;
            Parameters = parameters;
        }
        
        [HttpGet("~/dashboard/patients/{id}/personal-data")]
        public async Task<PatientPersonalData> Read(int id)
        {
            var period = Request.Query["period"];
            var date = Request.Query["date"];

            PatientPersonalData data = new PatientPersonalData();

            JsonResult res = (JsonResult)await new ActivitySummaryController(IPConfig, Parameters).Read(id, date, period);
            List<ActivitySummary> summaries = JArray.Parse(res.Value.ToString()).GetList<ActivitySummary>();
            
            data.TotalSteps = (from s in summaries select s.Steps).Sum();
            data.TotalCalories = (from s in summaries select s.CaloriesCategory.OutCalories).Sum();
            data.AverageFloors = (from s in summaries select s.Floors).Average();
            
            JsonResult resWeight = (JsonResult)await new WeightController(IPConfig, Parameters).Read(id, DateTime.Now.ToShortDateString());
            Weights weightToday = JObject.Parse(resWeight.Value.ToString()).GetObject<Weights>();

            resWeight = (JsonResult)await new WeightController(IPConfig, Parameters).Read(id, DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToShortDateString());
            Weights weightYesterday = JObject.Parse(resWeight.Value.ToString()).GetObject<Weights>();
            weightToday.Weight = 73;
            data.LastTwoWeights = new Weights[]{ weightToday, weightYesterday };

            return data;
        }
    }
}