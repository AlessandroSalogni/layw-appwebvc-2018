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
    public abstract class ChartDashboardController<TType> : Controller where TType : IComparable<TType>
    {
        private readonly IOptions<ServerIP> config;
        private readonly IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };
        private string PathGoals;
        private string PathSummary;
        private string Key;

        protected struct AchievedGoals
        {
            public TType Goal;
            public TType Summary;
            public string Name;
        }

        public ChartDashboardController(IOptions<ServerIP> config, string pathGoals, string pathSummary, string key)
        {
            this.config = config;
            PathGoals = pathGoals;
            PathSummary = pathSummary;
            Key = key;
        }

        [HttpGet("achieved")]
        public ActionResult ReadAchieved()
        {
            List<AchievedGoals> achievedGoalsgList = GetAchievedGoals(DateTime.Now.ToShortDateString().Replace('/', '-'));
            return Json((from x in achievedGoalsgList where x.Goal.CompareTo(x.Summary) <= 0 select x.Name).ToList());
        }

        [HttpGet("notachieved")]
        public ActionResult ReadNotAchieved()
        {
            List<AchievedGoals> achievedGoalsgList = GetAchievedGoals(DateTime.Now.ToShortDateString().Replace('/', '-'));
            return Json((from x in achievedGoalsgList where x.Goal.CompareTo(x.Summary) > 0 select x.Name).ToList());
        }

        [HttpGet("summary")]
        public ActionResult ReadSummary()
        {
            List<AchievedGoals> achievedGoalsgList = GetAchievedGoals(DateTime.Now.ToShortDateString().Replace('/', '-'));

            int notAchieved = achievedGoalsgList.Count(x => x.Goal.CompareTo(x.Summary) > 0);
            int achieved = achievedGoalsgList.Count - notAchieved;

            return Json(new List<object> { new { category = "Achieved", amount = achieved, color = "#00E100" }, new { category = "Not Achieved", amount = notAchieved, color = "#FF0000" } });
        }

        private List<AchievedGoals> GetAchievedGoals(string date)
        {
            List<AchievedGoals> achievedGoalsList = new List<AchievedGoals>();

            foreach (Patient patient in DashboardController.doctor.Patients)
            {
                string jsonResultGoals = Utils.Get(config.Value.GetTotalUrl() + "users/" + patient.Id + "/" + PathGoals + "?date=" + date);
                string jsonResultActivity = Utils.Get(config.Value.GetTotalUrl() + "users/" + patient.Id + "/" + PathSummary + "?date=18-07-2018"); //todo + date.ToShortDateString().Replace('/', '-'));

                JObject jsonGoals = JObject.Parse(jsonResultGoals);
                JObject jsonGoalsRoot = (jsonGoals.First as JProperty).Value as JObject;

                JObject jsonSummary = JObject.Parse(jsonResultActivity);
                var jsonSummaryRoot = (jsonSummary.First as JProperty).Value as JObject;

                AchievedGoals achievedGoals;
                achievedGoals.Goal = jsonGoalsRoot.GetValue("goal").Value<TType>();
                achievedGoals.Summary = jsonSummaryRoot.GetValue(Key).Value<TType>();
                achievedGoals.Name = patient.Name;

                achievedGoalsList.Add(achievedGoals);
            }

            return achievedGoalsList;
        }
    }

    [Route("~/dashboard/[controller]")]
    public class GoalStepsChartDashboardController : ChartDashboardController<int>
    {
        public GoalStepsChartDashboardController(IOptions<ServerIP> config) : base(config, "goals-steps-daily", "activity-summary", "steps") { }
    }

    [Route("~/dashboard/[controller]")]
    public class GoalWeightsChartDashboardController : ChartDashboardController<float>
    {
        public GoalWeightsChartDashboardController(IOptions<ServerIP> config) : base(config, "goals-weight", "weights", "weight") { }
    }

    [Route("~/dashboard/[controller]")]
    public class GoalCaloriesOutChartDashboardController : ChartDashboardController<float>
    {
        public GoalCaloriesOutChartDashboardController(IOptions<ServerIP> config) : base(config, "goals-calories-out", "activity-summary", "outCalories") { }
    }
}
