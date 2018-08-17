using System;
using System.Collections.Generic;
using System.Linq;
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
            public ActivitySummary ActivitySummary;
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
                agList.Add(GetAchievedGoals(patient.Id, DateTime.Now.ToShortDateString().Replace('/','-')));
            
            int notAchieved = agList.Count(x => x.GoalsStepsDaily.Goal > x.ActivitySummary.Steps);
            int ac = agList.Count - notAchieved;

            var query = from x in agList where x.GoalsStepsDaily.Goal > x.ActivitySummary.Steps select x.Name;
            var query2 = from x in agList where x.GoalsStepsDaily.Goal <= x.ActivitySummary.Steps select x.Name;
            
            if (Request.Query["achieved"].Equals("yes"))
                return Json(query2.ToList());
            else if (Request.Query["achieved"].Equals("no"))
                return Json(query.ToList());
            else
                return Json(new List<object> { new { category = "Achieved", amount = ac, color = "#00E100"}, new { category = "Not Achieved", amount = notAchieved, color = "#FF0000"} });
        }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        [HttpGet("~/dashboard/patients/{id}/[controller]/current")]
        public ActionResult Read(int id)
        {
            switch (Request.Query.Count)
            {
                case 0:
                    AchievedGoals ag = GetAchievedGoals(id, DateTime.Now.ToShortDateString().Replace('/', '-'));
                    return Json(new { steps = ag.ActivitySummary.Steps, day = ag.ActivitySummary.Date.ToShortDateString() });
                case 1:
                    AchievedGoals agDate = GetAchievedGoals(id, Request.Query["beginDate"]);
                    return Json(new { steps = agDate.ActivitySummary.Steps, day = agDate.ActivitySummary.Date.ToShortDateString() });
                case 2:
                    List<AchievedGoals> list = GetAchievedGoalsList(id, Request.Query["beginDate"], Request.Query["period"]);
                    List<object> listJson = new List<object>();
                    foreach (AchievedGoals agEl in list)
                    {
                        listJson.Add(new { steps = agEl.ActivitySummary.Steps, day = agEl.ActivitySummary.Date.ToShortDateString() });
                    }
                    return Json(listJson);
                default: return null;
            }
        }
        

        private AchievedGoals GetAchievedGoals(int id, string date)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };

            string jsonResultGoals = Utils.Get(config.Value.GetTotalUrl() + "users/" + id + "/goals-steps-daily?date=" + date);
            string jsonResultActivity = Utils.Get(config.Value.GetTotalUrl() + "users/" + id + "/activity-summary?date=16-07-2018"); //todo + date.ToShortDateString().Replace('/', '-'));

            JObject json = JObject.Parse(jsonResultGoals);
            JObject jsonObject = (JObject)json.GetValue("goals-steps-daily");

            JObject jsonAct = JObject.Parse(jsonResultActivity);
            JObject jsonObjectAct = (JObject)jsonAct.GetValue("activity-summary");

            AchievedGoals ag;
            ag.GoalsStepsDaily = JsonConvert.DeserializeObject<GoalsStepsDaily>(jsonObject.ToString(), dateTimeConverter);
            ag.ActivitySummary = JsonConvert.DeserializeObject<ActivitySummary>(jsonObjectAct.ToString(), dateTimeConverter);

            ag.Name = DashboardController.doctor.Patients.FirstOrDefault(x => x.Id == id).Name;

            return ag;
        }

        private List<AchievedGoals> GetAchievedGoalsList(int id, string beginDate, string period)
        {
            List<AchievedGoals> agList = new List<AchievedGoals>();
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };
            
            string jsonResultActivity = Utils.Get(config.Value.GetTotalUrl() + "users/" + id + "/activity-summary?date=23-07-2018&period=" + period);//todo beginDate
            
            JObject jsonAct = JObject.Parse(jsonResultActivity);
            JArray jsonArrayAct = (JArray)jsonAct.GetValue("activity-summaries");
            
            foreach(JObject jObj in jsonArrayAct)
            {
                AchievedGoals ag = new AchievedGoals();
                ag.ActivitySummary = JsonConvert.DeserializeObject<ActivitySummary>(jObj.ToString(), dateTimeConverter);
                
                string jsonResultGoals = Utils.Get(config.Value.GetTotalUrl() + "users/" + id + "/goals-steps-daily?date=" + ag.ActivitySummary.Date.ToShortDateString().Replace('/','-'));
                JObject json = (JObject)(JObject.Parse(jsonResultGoals)).GetValue("goals-steps-daily");
                ag.GoalsStepsDaily = JsonConvert.DeserializeObject<GoalsStepsDaily>(json.ToString(), dateTimeConverter);

                agList.Add(ag);
            }
            
            return agList;
        }
    }
    
}