using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{

    public class DashboardController : Controller
    {
        public const string SessionKeyName = "_Doctor";
        private readonly IOptions<ServerIP> config;

        public DashboardController(IOptions<ServerIP> config)
        {
            this.config = config;
        }

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
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var doctor = BuildDoctor();

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

            APIUtils.Post(config.Value.GetTotalUrl() + "doctors", jsonResult);

            JObject json = APIUtils.Get(config.Value.GetTotalUrl() + "users?doctor-id=" + doctor.EMail); //todo mettere path nel config
            doctor.Patients = ((JArray)json.GetValue("users")).GetList<Patient>();

            return doctor;
        }

    }
}