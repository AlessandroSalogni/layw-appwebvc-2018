using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.APIUtils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class GoalStepsDailyController : Controller, ICrudController
    {
        private readonly IOptions<ServerIP> config;
        
        public GoalStepsDailyController(IOptions<ServerIP> config)
        {
            this.config = config;
        }

        struct AchievedGoals
        {
            public GoalsStepsDaily GoalsStepsDaily;
            public int StepsDone;
            public string Name;
        }

        [HttpGet("~/dashboard/goalstepsdaily")]
        public ActionResult Read()
        {
            List<AchievedGoals> agList = new List<AchievedGoals>();

            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };

            foreach (Patient patient in DashboardController.doctor.Patients)
            {
                string jsonResultGoals = Utils.Get(config.Value.GetTotalUrl() + "users/" + patient.Id + "/goals-steps-daily/current");
                string jsonResultActivity = Utils.Get(config.Value.GetTotalUrl() + "users/" + patient.Id + "/activity-summary?date=16-07-2018");

                JObject json = JObject.Parse(jsonResultGoals);
                JObject jsonObject = (JObject)json.GetValue("goals-steps-daily");

                JObject jsonAct = JObject.Parse(jsonResultActivity);
                JObject jsonObjectAct = (JObject)jsonAct.GetValue("activity-summary");

                AchievedGoals ag;
                ag.GoalsStepsDaily = JsonConvert.DeserializeObject<GoalsStepsDaily>(jsonObject.ToString(), dateTimeConverter);
                ag.StepsDone = (int)jsonObjectAct.GetValue("steps");
                ag.Name = patient.Name;
                agList.Add(ag);
            }

            int notAchieved = agList.Count(x => x.GoalsStepsDaily.Goal > x.StepsDone);
            int achieved = agList.Count - notAchieved;

            var query = from x in agList where x.GoalsStepsDaily.Goal > x.StepsDone select x.Name;
            List<string> nameNotA = query.ToList();

            var query2 = from x in agList where x.GoalsStepsDaily.Goal <= x.StepsDone select x.Name;
            List<string> nameA = query2.ToList();

            return Json(new List<object> { new { category = "Achieved", amount = achieved, color = "#00E100", patients = nameA }, new { category = "Not Achieved", amount = notAchieved, color = "#FF0000", patients = nameNotA } });
        }

        /*
        [HttpGet("~/dashboard/goalstepsdaily")]
        public ActionResult Read(int id)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };

            string jsonResultGoals = Utils.Get(config.Value.GetTotalUrl() + "users/" + id + "/goals-steps-daily/current");
            string jsonResultActivity = Utils.Get(config.Value.GetTotalUrl() + "users/" + id + "/activity-summary?date=16-07-2018");

            JObject json = JObject.Parse(jsonResultGoals);
            JObject jsonObject = (JObject)json.GetValue("goals-steps-daily");

            JObject jsonAct = JObject.Parse(jsonResultActivity);
            JObject jsonObjectAct = (JObject)jsonAct.GetValue("activity-summary");

            AchievedGoals ag;
            ag.GoalsStepsDaily = JsonConvert.DeserializeObject<GoalsStepsDaily>(jsonObject.ToString(), dateTimeConverter);
            ag.StepsDone = (int)jsonObjectAct.GetValue("steps");
            ag.Name = DashboardController.doctor.Patients.FirstOrDefault(x => x.Id == id).Name;


            return Json(new List<object> { new { category = "Done", amount = ag.StepsDone, color = "#00E100" }, new { category = "To do", amount = ag.GoalsStepsDaily.Goal, color = "#FF0000" } });
        }
        */
    }
    
}