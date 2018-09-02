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
            var data = new PatientPersonalData
            {
                Patient = currentPatient
            };

            var jsonSummaryMonth = (JsonResult)await 
                new ActivitySummaryController(IPConfig, JsonStructureConfig).Read(currentPatient.Id, "23-06-2018", "1m"); //todo settare data oggi DateTime.Now.ToShortDateString()
            List<ActivitySummary> summariesMonth = JArray.Parse(jsonSummaryMonth.Value.ToString()).GetList<ActivitySummary>();

            var jsonSummaryWeek = (JsonResult)await
                new ActivitySummaryController(IPConfig, JsonStructureConfig).Read(currentPatient.Id, "23-06-2018", "1w");
            List<ActivitySummary> summariesWeek = JArray.Parse(jsonSummaryWeek.Value.ToString()).GetList<ActivitySummary>();

            data.TotalSteps.Month = (from s in summariesMonth select s.Steps).Sum();
            data.TotalSteps.Week = (from s in summariesWeek select s.Steps).Sum();

            data.TotalCalories.Month = (from s in summariesMonth select s.CaloriesCategory.OutCalories).Sum();
            data.TotalCalories.Week = (from s in summariesWeek select s.CaloriesCategory.OutCalories).Sum();

            data.AverageFloors.Month = (from s in summariesMonth select s.Floors).Average();
            data.AverageFloors.Week = (from s in summariesWeek select s.Floors).Average();

            var jsonWeightToday = (JsonResult)await
                new WeightController(IPConfig, JsonStructureConfig).Read(currentPatient.Id, DateTime.Now.ToShortDateString());
            data.WeightComparison.Today = JObject.Parse(jsonWeightToday.Value.ToString()).GetObject<PatientWeight>();

            var jsonWeightYesterday = (JsonResult)await 
                new WeightController(IPConfig, JsonStructureConfig).Read(currentPatient.Id, DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToShortDateString());
            data.WeightComparison.Yesterday = JObject.Parse(jsonWeightYesterday.Value.ToString()).GetObject<PatientWeight>();

            return View(data);
        }
    }
}