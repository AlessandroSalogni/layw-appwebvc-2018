using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public abstract class ChartPatientController<TType> : Controller where TType : IComparable<TType>
    {
        protected Configuration.JsonData GoalConfig { get; set; }
        protected Configuration.JsonData SummaryConfig { get; set; }
        protected Configuration.Parameters ParametersConfig { get; set; }

        protected readonly IOptions<ServerIP> config;
        private readonly IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };

        //todo mettere nel modello e togliere json dal metodo Read (stessa cosa per la dashboard) 
        public struct HistoryElement
        {
            public TType value;
            public TType goal;
            public string day;
        }

        public ChartPatientController(IOptions<ServerIP> config)
        {
            this.config = config;
        }

        public abstract Task<List<HistoryElement>> GetHistory(int id, string beginDate, string period);

        public async Task<ActionResult> GetJsonResult(int id, HttpRequest request)
        {
            List<HistoryElement> history = await GetHistory(id, request.Query["beginDate"], request.Query["period"]);
            return Json(history);
        }
    }

    public class StepsPatientController : ChartPatientController<int>
    {
        private IsoDateTimeConverter DateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };
        
        public StepsPatientController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> jsonStructure) : base(IPconfig)
        {
            GoalConfig = jsonStructure.Value.GoalsStepsDaily;
            SummaryConfig = jsonStructure.Value.StepsSummary;
            ParametersConfig = jsonStructure.Value.Parameters;
        }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<ActionResult> Read(int id)
        {
            return await GetJsonResult(id, Request);
        }

        public async override Task<List<HistoryElement>> GetHistory(int id, string beginDate, string period)
        {
            List<HistoryElement> history = new List<HistoryElement>();
            JObject obj = await APIUtils.GetAsync(config.Value.GetTotalUrlUser() + id + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate + "&" + ParametersConfig.Period + "=" + period);
            JArray array = (JArray)obj.GetValue(SummaryConfig.Root);

            //todo controllare se il dato esiste. Se non esiste ancora gestire l'eccezione
            JObject jsonGoals = await APIUtils.GetAsync(config.Value.GetTotalUrlUser() + id + "/" + GoalConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate);
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


    public class WeightPatientController : ChartPatientController<int>
    {
        private IsoDateTimeConverter DateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };

        public WeightPatientController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> jsonStructure) : base(IPconfig)
        {
            GoalConfig = jsonStructure.Value.GoalsWeight;
            SummaryConfig = jsonStructure.Value.WeightSummary;
            ParametersConfig = jsonStructure.Value.Parameters;
        }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<ActionResult> Read(int id)
        {
            return await GetJsonResult(id, Request);
        }

        public async override Task<List<HistoryElement>> GetHistory(int id, string beginDate, string period)
        {
            List<HistoryElement> history = new List<HistoryElement>();
            JObject obj = await APIUtils.GetAsync(config.Value.GetTotalUrlUser() + id + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate + "&" + ParametersConfig.Period + "=" + period);
            JArray array = (JArray)obj.GetValue(SummaryConfig.Root);

            JObject jsonGoals = await APIUtils.GetAsync(config.Value.GetTotalUrlUser() + id + "/" + GoalConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate);
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

    public class CaloriesOutPatientController : ChartPatientController<int>
    {
        private IsoDateTimeConverter DateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };

        public CaloriesOutPatientController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> jsonStructure) : base(IPconfig)
        {
            GoalConfig = jsonStructure.Value.GoalsCaloriesOut;
            SummaryConfig = jsonStructure.Value.CaloriesSummary;
            ParametersConfig = jsonStructure.Value.Parameters;
        }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<ActionResult> Read(int id)
        {
            return await GetJsonResult(id, Request);
        }

        public async override Task<List<HistoryElement>> GetHistory(int id, string beginDate, string period)
        {
            List<HistoryElement> history = new List<HistoryElement>();
            JObject obj = await APIUtils.GetAsync(config.Value.GetTotalUrlUser() + id + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate + "&" + ParametersConfig.Period + "=" + period);
            JArray array = (JArray)obj.GetValue(SummaryConfig.Root);

            JObject jsonGoals = await APIUtils.GetAsync(config.Value.GetTotalUrlUser() + id + "/" + GoalConfig.Url + "?" + ParametersConfig.Date + "=" + beginDate);
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