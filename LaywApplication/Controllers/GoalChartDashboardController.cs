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
        protected Configuration.JsonData GoalConfig { get; set; }
        protected Configuration.JsonData SummaryConfig { get; set; }
        protected Configuration.Parameters ParametersConfig { get; set; }

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
        public async Task<IEnumerable<Patient>> ReadNotAchieved()
        {
            List<AchievedGoals> achievedGoalsList = await GetAchievedGoalsAsync(DateTime.Now.ToShortDateString().Replace('/', '-'));
            List<Patient> patientList = new List<Patient>();
            foreach (string name in from x in achievedGoalsList where x.Goal.CompareTo(x.Summary) > 0 select x.Name)
            {
                Patient patient = new Patient();
                patient.Name = name;
                patientList.Add(patient);
            }

            return patientList;
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
        public GoalStepsChartDashboardController(IOptions<ServerIP> configIP, IOptions<JsonStructure> configJSON) : base(configIP)
        {
            GoalConfig = configJSON.Value.GoalsStepsDaily;
            SummaryConfig = configJSON.Value.StepsSummary;
            ParametersConfig = configJSON.Value.Parameters;
        }


        protected override async Task<int> GetPatientGoalAsync(int patientId, string date)
        {
            JObject jsonGoals = await Utils.GetAsync(config.Value.GetTotalUrlUser() + patientId + "/" + GoalConfig.Url + "?" + ParametersConfig.Date + "=" + date);
            JObject jsonGoalsRoot = jsonGoals.GetValue(GoalConfig.Root) as JObject;

            return jsonGoalsRoot.GetValue(GoalConfig.Key).Value<int>();
        }

        protected override async Task<int> GetPatientSummaryAsync(int patientId, string date)
        {
            JObject jsonSummary = await Utils.GetAsync(config.Value.GetTotalUrlUser() + patientId + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=18-07-2018"); //todo + date.ToShortDateString().Replace('/', '-'));
            JObject jsonSummaryRoot = jsonSummary.GetValue(SummaryConfig.Root) as JObject;

            return jsonSummaryRoot.GetValue(SummaryConfig.Key).Value<int>();
        }
    }

    [Route("~/dashboard/[controller]")]
    public class GoalWeightsChartDashboardController : GoalChartDashboardController<float>
    {
        public GoalWeightsChartDashboardController(IOptions<ServerIP> configIP, IOptions<JsonStructure> configJSON) : base(configIP)
        {
            GoalConfig = configJSON.Value.GoalsWeight;
            SummaryConfig = configJSON.Value.WeightSummary;
            ParametersConfig = configJSON.Value.Parameters;
        }

        protected override async Task<float> GetPatientGoalAsync(int patientId, string date)
        {
            JObject jsonGoals = await Utils.GetAsync(config.Value.GetTotalUrlUser() + patientId + "/" + GoalConfig.Url + "?" + ParametersConfig.Date + "=" + date);
            JObject jsonGoalsRoot = jsonGoals.GetValue(GoalConfig.Root) as JObject;

            return jsonGoalsRoot.GetValue(GoalConfig.Key).Value<float>();
        }

        protected override async Task<float> GetPatientSummaryAsync(int patientId, string date)
        {
            JObject jsonSummary = await Utils.GetAsync(config.Value.GetTotalUrlUser() + patientId + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=18-07-2018"); //todo + date.ToShortDateString().Replace('/', '-'));
            JObject jsonSummaryRoot = jsonSummary.GetValue(SummaryConfig.Root) as JObject;

            return jsonSummaryRoot.GetValue(SummaryConfig.Key).Value<float>();
        }
    }

    [Route("~/dashboard/[controller]")]
    public class GoalCaloriesOutChartDashboardController : GoalChartDashboardController<float>
    {
        public GoalCaloriesOutChartDashboardController(IOptions<ServerIP> configIP, IOptions<JsonStructure> configJSON) : base(configIP)
        {
            GoalConfig = configJSON.Value.GoalsCaloriesOut;
            SummaryConfig = configJSON.Value.CaloriesSummary;
            ParametersConfig = configJSON.Value.Parameters;
        }

        protected override async Task<float> GetPatientGoalAsync(int patientId, string date)
        {
            JObject jsonGoals = await Utils.GetAsync(config.Value.GetTotalUrlUser() + patientId + "/" + GoalConfig.Url + "?" + ParametersConfig.Date + "=" + date);
            JObject jsonGoalsRoot = jsonGoals.GetValue(GoalConfig.Root) as JObject;

            return jsonGoalsRoot.GetValue(GoalConfig.Key).Value<float>();
        }

        protected override async Task<float> GetPatientSummaryAsync(int patientId, string date)
        {
            JObject jsonSummary = await Utils.GetAsync(config.Value.GetTotalUrlUser() + patientId + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=18-07-2018"); //todo + date.ToShortDateString().Replace('/', '-'));
            JObject jsonSummaryRoot = jsonSummary.GetValue(SummaryConfig.Root) as JObject;
            JObject jsonCaloriesCategory = (JObject)jsonSummaryRoot.GetValue((SummaryConfig as CaloriesSummary).Object);

            return jsonCaloriesCategory.GetValue(SummaryConfig.Key).Value<float>();
        }
    }
}
