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
        struct AchievedGoals
        {
            public GoalsStepsDaily GoalsStepsDaily;
            public int StepsDone;
            public string Name;
        }
        private readonly IOptions<ServerIP> config;
        
        public GoalStepsDailyController(IOptions<ServerIP> config)
        {
            this.config = config;
        }

        [HttpGet("~/dashboard/[controller]")]
        public ActionResult Read()
        {
            List<AchievedGoals> agList = new List<AchievedGoals>();
            
            foreach (Patient patient in DashboardController.doctor.Patients)
            {
                agList.Add(GetAchievedGoals(patient.Id));
            }

            int notAchieved = agList.Count(x => x.GoalsStepsDaily.Goal > x.StepsDone);
            int ac = agList.Count - notAchieved;

            var query = from x in agList where x.GoalsStepsDaily.Goal > x.StepsDone select x.Name;
            var query2 = from x in agList where x.GoalsStepsDaily.Goal <= x.StepsDone select x.Name;
            
            if (Request.Query["achieved"].Equals("yes"))
                return Json(query2.ToList());
            else if (Request.Query["achieved"].Equals("no"))
                return Json(query.ToList());
            else
                return Json(new List<object> { new { category = "Achieved", amount = ac, color = "#00E100"}, new { category = "Not Achieved", amount = notAchieved, color = "#FF0000"} });
        }

        [HttpGet("~/dashboard/patients/{id}/[controller]/current")]
        public ActionResult Read(int id)
        {
            AchievedGoals ag = GetAchievedGoals(id);
            return Json(new List<object> { new { category = "Done", amount = ag.StepsDone, color = "#00E100" }, new { category = "To do", amount = ag.GoalsStepsDaily.Goal, color = "#FF0000" } });
        }

        private AchievedGoals GetAchievedGoals(int id)
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

            return ag;
        }
    }
    
}