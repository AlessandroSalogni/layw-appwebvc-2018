using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LaywApplication.ViewComponents
{
    public class PatientActivitiesViewComponent : BaseViewComponent
    {
        public PatientActivitiesViewComponent(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig) { }

        public async Task<IViewComponentResult> InvokeAsync(Patient currentPatient)
        {
            ViewBag.AerobicFunction = new AerobicFunction(currentPatient);

            JObject activitiesJson = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + currentPatient.Id 
                + JsonStructureConfig.Activities.Url + "?" + JsonStructureConfig.QueryParams.Date + "=14-06-2018"); //TODO mettere data di oggi
            return View(((JArray)activitiesJson[JsonStructureConfig.Activities.Root]).GetList<Activity>());
        }
    }
}