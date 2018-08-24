using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class ActivitiesController : Controller
    {
        public struct ActivityId
        {
            public int id;
            public string name;
        }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<IActionResult> GetHistoryMy(int id)
        {
            List<ActivityId> activities = new List<ActivityId>();
            JObject obj = await APIUtils.GetAsync("http://localhost:4567/api/v1.0/users/" + id + "/" + "activities" + "?date=14-06-2018"); // + ParametersConfig.Date + "=" + beginDate + "&" + ParametersConfig.Period + "=" + period);
            JArray array = (JArray)obj.GetValue("activities");

            foreach (JObject element in array)
            {
                ActivityId activityId = new ActivityId
                {
                    id = array.IndexOf(element),
                    name = element.GetValue("activityName").ToString()
                };
                activities.Add(activityId);
            }

            return Json(new { activities });
        }


        [HttpGet("~/dashboard/patients/{patientId}/[controller]/{activityId}")]
        public async Task<IActionResult> GetHistoryMy(int patientId, int activityId)
        {
            List<ActivityId> activities = new List<ActivityId>();
            JObject obj = await APIUtils.GetAsync("http://localhost:4567/api/v1.0/users/" + patientId + "/" + "activities" + "?date=14-06-2018"); // + ParametersConfig.Date + "=" + beginDate + "&" + ParametersConfig.Period + "=" + period);
            JArray array = (JArray)obj.GetValue("activities");

            JObject el = (JObject)array[activityId];
            JArray arrayBeats = (JArray)el.GetValue("heartBeats");
            
            return Json(arrayBeats);
        }

    }
}