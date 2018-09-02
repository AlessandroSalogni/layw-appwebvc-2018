using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.PatientController;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class PatientPersonalDataViewComponent : ViewComponent
    {
        private readonly ServerIP IPConfig;
        private readonly JsonStructure JsonStructureConfig;

        public PatientPersonalDataViewComponent(ServerIP IPConfig, JsonStructure jsonStructureConfig)
        {
            this.IPConfig = IPConfig;
            JsonStructureConfig = jsonStructureConfig;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(Patient currentPatient)
        {
            List<ActivitySummary> summariesMonth = await GetSummaryListAsync(currentPatient.Id, "23-06-2018", "1m"); //todo settare data oggi DateTime.Now.ToShortDateString()
            List<ActivitySummary> summariesWeek = await GetSummaryListAsync(currentPatient.Id, "23-06-2018", "1w");

            var data = new PatientPersonalData
            {
                Patient = currentPatient
            };

            data.TotalSteps.Month = (from s in summariesMonth select s.Steps).Sum();
            data.TotalSteps.Week = (from s in summariesWeek select s.Steps).Sum();

            data.TotalCalories.Month = (from s in summariesMonth select s.CaloriesCategory.OutCalories).Sum();
            data.TotalCalories.Week = (from s in summariesWeek select s.CaloriesCategory.OutCalories).Sum();

            data.AverageFloors.Month = (from s in summariesMonth select s.Floors).Average();
            data.AverageFloors.Week = (from s in summariesWeek select s.Floors).Average();

            data.WeightComparison.Today = await GetWeightAsync(currentPatient.Id, DateTime.Now.ToShortDateString());
            data.WeightComparison.Yesterday = await GetWeightAsync(currentPatient.Id, DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToShortDateString());

            return View(data);
        }

        private async Task<PatientWeight> GetWeightAsync(int id, string date)
        {
            var jsonWeight = (JsonResult)await new WeightController(IPConfig, JsonStructureConfig).Read(id, date);
            return JObject.Parse(jsonWeight.Value.ToString()).GetObject<PatientWeight>();
        }

        private async Task<List<ActivitySummary>> GetSummaryListAsync(int id, string date, string period)
        {
            var jsonSummary = (JsonResult)await new ActivitySummaryController(IPConfig, JsonStructureConfig).Read(id, date, period);
            return JArray.Parse(jsonSummary.Value.ToString()).GetList<ActivitySummary>();
        }
    }
}