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
    public abstract class GoalChartDashboardController<TType> : Controller where TType : IComparable<TType>
    {
        protected readonly IOptions<ServerIP> config;
        private readonly IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };

        protected struct AchievedGoals
        {
            public TType Goal;
            public TType Summary;
            public string Name;
        }

        public GoalChartDashboardController(IOptions<ServerIP> config)
        {
            this.config = config;
        }

        [HttpGet("achieved")]
        public async Task<ActionResult> ReadAchieved()
        {
            List<AchievedGoals> achievedGoalsList = await GetAchievedGoalsAsync(DateTime.Now.ToShortDateString().Replace('/', '-'));
            return Json((from x in achievedGoalsList where x.Goal.CompareTo(x.Summary) <= 0 select x.Name).ToList());
        }

        [HttpGet("notachieved")]
        public async Task<ActionResult> ReadNotAchieved()
        {
            List<AchievedGoals> achievedGoalsList = await GetAchievedGoalsAsync(DateTime.Now.ToShortDateString().Replace('/', '-'));
            return Json((from x in achievedGoalsList where x.Goal.CompareTo(x.Summary) > 0 select x.Name).ToList());
        }

        [HttpGet("summary")]
        public async Task<ActionResult> ReadSummary()
        {
            List<AchievedGoals> achievedGoalsList = await GetAchievedGoalsAsync(DateTime.Now.ToShortDateString().Replace('/', '-'));

            int notAchieved = achievedGoalsList.Count(x => x.Goal.CompareTo(x.Summary) > 0);
            int achieved = achievedGoalsList.Count - notAchieved;

            return Json(new List<object> { new { category = "Achieved", amount = achieved, color = "#00E100" }, new { category = "Not Achieved", amount = notAchieved, color = "#FF0000" } });
        }

        protected abstract Task<TType> GetPatientGoalAsync(int patientId, string date);
        protected abstract Task<TType> GetPatientSummaryAsync(int patientId, string date);

        private async Task<List<AchievedGoals>> GetAchievedGoalsAsync(string date)
        {
            List<AchievedGoals> achievedGoalsList = new List<AchievedGoals>();

            foreach (Patient patient in DashboardController.doctor.Patients)
            {
                AchievedGoals achievedGoals;
                achievedGoals.Goal = await GetPatientGoalAsync(patient.Id, date);
                achievedGoals.Summary = await GetPatientSummaryAsync(patient.Id, date);
                achievedGoals.Name = patient.Name;

                achievedGoalsList.Add(achievedGoals);
            }

            return achievedGoalsList;
        }
    }

    [Route("~/dashboard/[controller]")]
    public class GoalStepsChartDashboardController : GoalChartDashboardController<int>
    {
        public GoalStepsChartDashboardController(IOptions<ServerIP> config) : base(config) { }

        protected override async Task<int> GetPatientGoalAsync(int patientId, string date)
        {
            string jsonResultGoals = await Utils.GetAsync(config.Value.GetTotalUrl() + "users/" + patientId + "/goals-steps-daily?date=" + date);

            JObject jsonGoals = JObject.Parse(jsonResultGoals);
            JObject jsonGoalsRoot = (jsonGoals.First as JProperty).Value as JObject;

            return jsonGoalsRoot.GetValue("goal").Value<int>();
        }

        protected override async Task<int> GetPatientSummaryAsync(int patientId, string date)
        {
            string jsonResultActivity = await Utils.GetAsync(config.Value.GetTotalUrl() + "users/" + patientId + "/activity-summary?date=18-07-2018"); //todo + date.ToShortDateString().Replace('/', '-'));

            JObject jsonSummary = JObject.Parse(jsonResultActivity);
            JObject jsonSummaryRoot = (jsonSummary.First as JProperty).Value as JObject;

            return jsonSummaryRoot.GetValue("steps").Value<int>();
        }
    }

    [Route("~/dashboard/[controller]")]
    public class GoalWeightsChartDashboardController : GoalChartDashboardController<float>
    {
        public GoalWeightsChartDashboardController(IOptions<ServerIP> config) : base(config) { }

        protected override async Task<float> GetPatientGoalAsync(int patientId, string date)
        {
            string jsonResultGoals = await Utils.GetAsync(config.Value.GetTotalUrl() + "users/" + patientId + "/goals-weight?date=" + date);

            JObject jsonGoals = JObject.Parse(jsonResultGoals);
            JObject jsonGoalsRoot = (jsonGoals.First as JProperty).Value as JObject;

            return jsonGoalsRoot.GetValue("goal").Value<float>();
        }

        protected override async Task<float> GetPatientSummaryAsync(int patientId, string date)
        {
            string jsonResultActivity = await Utils.GetAsync(config.Value.GetTotalUrl() + "users/" + patientId + "/weights?date=18-07-2018"); //todo + date.ToShortDateString().Replace('/', '-'));

            JObject jsonSummary = JObject.Parse(jsonResultActivity);
            JObject jsonSummaryRoot = (jsonSummary.First as JProperty).Value as JObject;

            return jsonSummaryRoot.GetValue("weight").Value<float>();
        }
    }

    [Route("~/dashboard/[controller]")]
    public class GoalCaloriesOutChartDashboardController : GoalChartDashboardController<float>
    {
        public GoalCaloriesOutChartDashboardController(IOptions<ServerIP> config) : base(config) { }

        protected override async Task<float> GetPatientGoalAsync(int patientId, string date)
        {
            string jsonResultGoals = await Utils.GetAsync(config.Value.GetTotalUrl() + "users/" + patientId + "/goals-calories-out?date=" + date);

            JObject jsonGoals = JObject.Parse(jsonResultGoals);
            JObject jsonGoalsRoot = (jsonGoals.First as JProperty).Value as JObject;

            return jsonGoalsRoot.GetValue("goal").Value<float>();
        }

        protected override async Task<float> GetPatientSummaryAsync(int patientId, string date)
        {
            string jsonResultActivity = await Utils.GetAsync(config.Value.GetTotalUrl() + "users/" + patientId + "/activity-summary?date=18-07-2018"); //todo + date.ToShortDateString().Replace('/', '-'));

            JObject jsonSummary = JObject.Parse(jsonResultActivity);
            JObject jsonSummaryRoot = (jsonSummary.First as JProperty).Value as JObject;
            JObject jsonCaloriesCategory = (JObject)jsonSummaryRoot.GetValue("caloriesCategory");

            return jsonCaloriesCategory.GetValue("outCalories").Value<float>();
        }
    }
}
