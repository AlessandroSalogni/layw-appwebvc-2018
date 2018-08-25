using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;


namespace LaywApplication.Controllers
{
    public abstract class GoalPatientController<TType> : BasePatientController where TType : IComparable<TType>
    {
        protected Configuration.JsonData GoalConfig { get; set; }
        protected Configuration.JsonData SummaryConfig { get; set; }

        //todo mettere nel modello e togliere json dal metodo Read (stessa cosa per la dashboard) 
        public struct HistoryElement
        {
            public TType value;
            public TType goal;
            public string day;
        }

        public GoalPatientController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> parameters) : base(IPconfig, parameters) { }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<ActionResult> Read(int id)
        {
            return Json(await GetHistory(id, Request.Query["beginDate"], Request.Query["period"]));
        }

        public abstract Task<List<HistoryElement>> GetHistory(int id, string beginDate, string period);
    }

    public class StepsPatientController : GoalPatientController<int>
    {
        public StepsPatientController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> jsonStructure) : base(IPconfig, jsonStructure)
        {
            GoalConfig = jsonStructure.Value.GoalsStepsDaily;
            SummaryConfig = jsonStructure.Value.StepsSummary;
        }

        public async override Task<List<HistoryElement>> GetHistory(int id, string beginDate, string period)
        {
            List<HistoryElement> history = new List<HistoryElement>();
            JObject obj = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate + "&" + ParametersConfig.Period + "=" + period);
            JArray array = (JArray)obj.GetValue(SummaryConfig.Root);

            //todo controllare se il dato esiste. Se non esiste ancora gestire l'eccezione
            JObject jsonGoals = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/" + GoalConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate);
            JObject jsonGoalsRoot = jsonGoals.GetValue(GoalConfig.Root) as JObject;
            var goalJson = jsonGoalsRoot.GetValue(GoalConfig.Key).Value<int>();

            foreach (JObject element in array)
            {
                HistoryElement historyElement = new HistoryElement
                {
                    goal = goalJson,
                    value = element.GetValue(SummaryConfig.Key).Value<int>(),
                    day = element.GetValue("date").Value<string>()
                };

                history.Add(historyElement);
            }
            return history;
        }
    }

    public class WeightPatientController : GoalPatientController<int>
    {
        public WeightPatientController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> jsonStructure) : base(IPconfig, jsonStructure)
        {
            GoalConfig = jsonStructure.Value.GoalsWeight;
            SummaryConfig = jsonStructure.Value.WeightSummary;
        }

        public async override Task<List<HistoryElement>> GetHistory(int id, string beginDate, string period)
        {
            List<HistoryElement> history = new List<HistoryElement>();
            JObject obj = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate + "&" + ParametersConfig.Period + "=" + period);
            JArray array = (JArray)obj.GetValue(SummaryConfig.Root);

            JObject jsonGoals = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/" + GoalConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate);
            JObject jsonGoalsRoot = jsonGoals.GetValue(GoalConfig.Root) as JObject;
            var goalJson = jsonGoalsRoot.GetValue(GoalConfig.Key).Value<int>();

            foreach (JObject element in array)
            {
                HistoryElement historyElement = new HistoryElement
                {
                    goal = goalJson,
                    value = element.GetValue(SummaryConfig.Key).Value<int>(),
                    day = element.GetValue("date").Value<string>()
                };

                history.Add(historyElement);
            }
            return history;
        }
    }

    public class CaloriesOutPatientController : GoalPatientController<int>
    {
        public CaloriesOutPatientController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> jsonStructure) : base(IPconfig, jsonStructure)
        {
            GoalConfig = jsonStructure.Value.GoalsCaloriesOut;
            SummaryConfig = jsonStructure.Value.CaloriesSummary;
        }

        public async override Task<List<HistoryElement>> GetHistory(int id, string beginDate, string period)
        {
            List<HistoryElement> history = new List<HistoryElement>();
            JObject obj = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate + "&" + ParametersConfig.Period + "=" + period);
            JArray array = (JArray)obj.GetValue(SummaryConfig.Root);

            JObject jsonGoals = await APIUtils.GetAsync(IPconfig.GetTotalUrlUser() + id + "/" + GoalConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate);
            JObject jsonGoalsRoot = jsonGoals.GetValue(GoalConfig.Root) as JObject;
            var goalJson = jsonGoalsRoot.GetValue(GoalConfig.Key).Value<int>();

            foreach (JObject element in array)
            {
                HistoryElement historyElement = new HistoryElement
                {
                    goal = goalJson,
                    value = (element.GetValue(((CaloriesSummary)SummaryConfig).Object) as JObject).GetValue(SummaryConfig.Key).Value<int>(),
                    day = element.GetValue("date").Value<string>()
                };

                history.Add(historyElement);
            }
            return history;
        }
    }
}