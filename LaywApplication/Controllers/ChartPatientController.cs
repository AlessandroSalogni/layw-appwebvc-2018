using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.APIUtils;
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
        protected Configuration.JsonData SummaryConfig { get; set; }
        protected Configuration.Parameters ParametersConfig { get; set; }

        protected readonly IOptions<ServerIP> config;
        private readonly IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };

        public struct HistoryElement
        {
            public TType value;
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
            List<object> result = new List<object>();

            foreach (HistoryElement historyElement in history)
                result.Add(new { historyElement.value, historyElement.day });

            return Json(result);
        }
    }

    public class StepsPatientController : ChartPatientController<int>
    {
        private IsoDateTimeConverter DateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };
        
        public StepsPatientController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> jsonStructure) : base(IPconfig)
        {
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
            JObject obj = await Utils.GetAsync(config.Value.GetTotalUrlUser() + id + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=27-06-2018&" + ParametersConfig.Period + "=" + period);//todo beginDate
            JArray array = (JArray)obj.GetValue(SummaryConfig.Root);

            foreach (JObject element in array)
            {
                HistoryElement historyElement = new HistoryElement
                {
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
            JObject obj = await Utils.GetAsync(config.Value.GetTotalUrlUser() + id + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=27-06-2018&" + ParametersConfig.Period + "=" + period);//todo beginDate
            JArray array = (JArray)obj.GetValue(SummaryConfig.Root);

            foreach (JObject element in array)
            {
                HistoryElement historyElement = new HistoryElement
                {
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
            JObject obj = await Utils.GetAsync(config.Value.GetTotalUrlUser() + id + "/" + SummaryConfig.Url + "?" + ParametersConfig.Date + "=27-06-2018&" + ParametersConfig.Period + "=" + period);//todo beginDate
            JArray array = (JArray)obj.GetValue(SummaryConfig.Root);

            foreach (JObject element in array)
            {
                HistoryElement historyElement = new HistoryElement
                {
                    value = (element.GetValue(((CaloriesSummary)SummaryConfig).Object) as JObject).GetValue(SummaryConfig.Key).Value<int>(),
                    day = element.GetValue("date").Value<string>()
                };

                history.Add(historyElement);
            }
            return history;
        }
    }
}