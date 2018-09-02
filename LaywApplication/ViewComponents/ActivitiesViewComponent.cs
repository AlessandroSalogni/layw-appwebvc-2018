using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LaywApplication.ViewComponents
{
    public class ActivitiesViewComponent : ViewComponent
    {
        private readonly ServerIP IPConfig;
        private readonly QueryParams QueryParamsConfig;
        private readonly JsonData ActivityJsonData;

        public ActivitiesViewComponent(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
        {
            this.IPConfig = IPConfig;
            QueryParamsConfig = jsonStructureConfig.QueryParams;
            ActivityJsonData = jsonStructureConfig.Activities;
        }

        public async Task<IViewComponentResult> InvokeAsync(Patient currentPatient)
        {
            ViewBag.AerobicFunction = new AerobicFunction(currentPatient);

            JObject activitiesJson = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + currentPatient.Id 
                + ActivityJsonData.Url + "?" + QueryParamsConfig.Date + "=14-06-2018"); //TODO mettere data di oggi
            return View(((JArray)activitiesJson[ActivityJsonData.Root]).GetList<Activity>());
        }
    }
}