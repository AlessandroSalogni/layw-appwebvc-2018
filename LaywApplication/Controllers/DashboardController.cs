using System;
using System.Linq;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class DashboardController : BaseController
    {
        public const string SessionKeyName = "_Doctor";

        public DashboardController(ServerIP IPConfig) : base(IPConfig) { }

        [HttpGet("~/dashboard")]
        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var doctor = BuildDoctor();

                HttpContext.Session.Set(SessionKeyName, doctor);

                return View(doctor);
            }
            else
                return Redirect("~/signin");
        }

        [HttpGet("~/dashboard/patients/{id}")]
        public IActionResult Patient(int id)
        {
            //TODO oltre a verificare l'autenticazione, verificare che il paziente appartenga veramente al medico. in caso contrario non restituire la view
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var doctor = HttpContext.Session.Get<Doctor>(SessionKeyName);
                doctor.Patients.ForEach(x => x.AerobicFunction = new AerobicFunction(x));

                ViewBag.CurrentPatient = doctor.Patients.FirstOrDefault(x => x.Id == id);
                return View("Patient", doctor);
            }
            else
                return Redirect("~/signin");
        }

        private Doctor BuildDoctor() //todo ancora da sistmare
        {
            var doctor = new Doctor(
                User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value,
                User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value,
                new Uri(User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri").Value));

            string jsonResult = "{\"doctor\": {\"name\": \"" + doctor.Name + "\", \"email\": \"" + doctor.EMail + "\"}}";

            APIUtils.Post(IPConfig.GetTotalUrl() + "doctors", jsonResult);

            JObject json = APIUtils.Get(IPConfig.GetTotalUrl() + "users?doctor-id=" + doctor.EMail); //todo mettere path nel config
            doctor.Patients = ((JArray)json.GetValue("users")).GetList<Patient>();
            doctor.Patients.ForEach(x => x.AerobicFunction = new AerobicFunction(x));

            return doctor;
        }

    }
}