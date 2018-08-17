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
    public class GoalWeightsController : Controller
    {
        struct AchievedGoals
        {
            public GoalsWeight GoalsWeight;
            public Weights Weight;
            public string Name;
        }

        private readonly IOptions<ServerIP> config;

        public GoalWeightsController(IOptions<ServerIP> config)
        {
            this.config = config;
        }

        [HttpGet("~/dashboard/[controller]")]
        public ActionResult Read()
        {
            List<AchievedGoals> agList = new List<AchievedGoals>();

            foreach (Patient patient in DashboardController.doctor.Patients)
                agList.Add(GetAchievedGoals(patient.Id, DateTime.Now.ToShortDateString().Replace('/', '-')));

            int notAchieved = agList.Count(x => x.GoalsWeight.Goal <= x.Weight.Weight);
            int ac = agList.Count - notAchieved;

            var query = from x in agList where x.GoalsWeight.Goal <= x.Weight.Weight select x.Name;
            var query2 = from x in agList where x.GoalsWeight.Goal > x.Weight.Weight select x.Name;

            if (Request.Query["achieved"].Equals("yes"))
                return Json(query2.ToList());
            else if (Request.Query["achieved"].Equals("no"))
                return Json(query.ToList());
            else
                return Json(new List<object> { new { category = "Achieved", amount = ac, color = "#00E100" }, new { category = "Not Achieved", amount = notAchieved, color = "#FF0000" } });
        }
        private AchievedGoals GetAchievedGoals(int id, string date)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };

            string jsonResultGoals = Utils.Get(config.Value.GetTotalUrl() + "users/" + id + "/goals-weight?date=" + date);
            string jsonResultActivity = Utils.Get(config.Value.GetTotalUrl() + "users/" + id + "/weights?date=18-07-2018"); //todo + date.ToShortDateString().Replace('/', '-'));

            JObject json = JObject.Parse(jsonResultGoals);
            JObject jsonObject = (JObject)json.GetValue("goals-weights");

            JObject jsonWeight = JObject.Parse(jsonResultActivity);
            JObject jsonObjectWeight = (JObject)jsonWeight.GetValue("weights");

            AchievedGoals ag;
            ag.GoalsWeight = JsonConvert.DeserializeObject<GoalsWeight>(jsonObject.ToString(), dateTimeConverter);
            ag.Weight = JsonConvert.DeserializeObject<Weights>(jsonObjectWeight.ToString(), dateTimeConverter);

            ag.Name = DashboardController.doctor.Patients.FirstOrDefault(x => x.Id == id).Name;

            return ag;
        }
    }
}