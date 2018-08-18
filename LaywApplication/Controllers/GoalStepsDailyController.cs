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

//namespace LaywApplication.Controllers
//{
//    [Route("~/dashboard/[controller]")]
//    public class GoalStepsChartDashboardController : ChartDashboardController<int>
//    {     
//        public GoalStepsChartDashboardController(IOptions<ServerIP> config) : base(config, "goals-steps-daily", "activity-summary", "steps") { }

        //[HttpGet("~/dashboard/patients/{id}/[controller]")]
        //[HttpGet("~/dashboard/patients/{id}/[controller]/current")]
        //public override ActionResult Read(int id)
        //{
            //switch (Request.Query.Count)
            //{
            //    case 0:
            //        AchievedGoals ag = GetAchievedGoals(id, DateTime.Now.ToShortDateString().Replace('/', '-'));
            //        return Json(new { steps = ag.ActivitySummary.Steps, day = ag.ActivitySummary.Date.ToShortDateString() });
            //    case 1:
            //        AchievedGoals agDate = GetAchievedGoals(id, Request.Query["beginDate"]);
            //        return Json(new { steps = agDate.ActivitySummary.Steps, day = agDate.ActivitySummary.Date.ToShortDateString() });
            //    case 2:
            //        List<AchievedGoals> list = GetAchievedGoalsList(id, Request.Query["beginDate"], Request.Query["period"]);
            //        List<object> listJson = new List<object>();
            //        foreach (AchievedGoals agEl in list)
            //        {
            //            listJson.Add(new { steps = agEl.ActivitySummary.Steps, day = agEl.ActivitySummary.Date.ToShortDateString() });
            //        }
            //        return Json(listJson);
            //    default: return null;
            //}
        //    return null;
        //}
        
        //private List<AchievedGoals> GetAchievedGoalsList(int id, string beginDate, string period)
        //{
        //    List<AchievedGoals> agList = new List<AchievedGoals>();
        //    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };
            
        //    string jsonResultActivity = Utils.Get(config.Value.GetTotalUrl() + "users/" + id + "/activity-summary?date=23-07-2018&period=" + period);//todo beginDate
            
        //    JObject jsonAct = JObject.Parse(jsonResultActivity);
        //    JArray jsonArrayAct = (JArray)jsonAct.GetValue("activity-summaries");
            
        //    foreach(JObject jObj in jsonArrayAct)
        //    {
        //        AchievedGoals ag = new AchievedGoals();
        //        ag.ActivitySummary = JsonConvert.DeserializeObject<ActivitySummary>(jObj.ToString(), dateTimeConverter);
                
        //        string jsonResultGoals = Utils.Get(config.Value.GetTotalUrl() + "users/" + id + "/goals-steps-daily?date=" + ag.ActivitySummary.Date.ToShortDateString().Replace('/','-'));
        //        JObject json = (JObject)(JObject.Parse(jsonResultGoals)).GetValue("goals-steps-daily");
        //        ag.GoalsStepsDaily = JsonConvert.DeserializeObject<GoalsStepsDaily>(json.ToString(), dateTimeConverter);

        //        agList.Add(ag);
        //    }
            
        //    return agList;
        //}
//    }
    
//}