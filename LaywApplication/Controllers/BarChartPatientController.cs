using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Controllers.APIUtils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class BarChartPatientController : Controller, ICrudController
    {
        public struct Chart
        {
            public int value;
            public string day;
        }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<ActionResult> Read(int id)
        {
            List<Chart> list = await GetChart(id, Request.Query["beginDate"], Request.Query["period"]);
            List<object> listJson = new List<object>();
            foreach (Chart agEl in list)
            {
                listJson.Add(new { steps = agEl.value, day = agEl.day }); //ActivitySummary.Date.ToShortDateString()
            }
            return Json(listJson);
        }

        
        public async Task<List<Chart>> GetChart(int id, string beginDate, string period)
        {
            List<Chart> agList = new List<Chart>();
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };

            JObject jsonResultActivity = await Utils.GetAsync("http://localhost:4567/api/v1.0/users/" + id + "/activity-summary?date=23-07-2018&period=" + period);//todo beginDate
            
            JArray jsonArrayAct = (JArray)jsonResultActivity.GetValue("activity-summary");

            foreach (JObject jObj in jsonArrayAct)
            {
                Chart ag = new Chart();
                var act = (JsonConvert.DeserializeObject<ActivitySummary>(jObj.ToString(), dateTimeConverter));
                ag.value = act.Steps;
                ag.day = act.Date.ToShortDateString().Replace('/', '-');
                
                agList.Add(ag);
            }

            return agList;
        }

        public ActionResult Read()
        {
            throw new NotImplementedException();
        }

        ActionResult ICrudController.Read(int id)
        {
            throw new NotImplementedException();
        }
    }
}