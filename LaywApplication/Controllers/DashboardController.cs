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
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{

    public class DashboardController : Controller
    {
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
                Doctor doctor = new Doctor(
                User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value,
                User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value,
                new Uri(User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri").Value));

                string jsonResult = "{\"doctor\": {\"name\": \"" + doctor.Name + "\", \"email\": \"" + doctor.EMail + "\"}}";
                Utils.Post(config.Value.GetTotalUrl() + "doctors", jsonResult);

                jsonResult = Utils.Get(config.Value.GetTotalUrl() + "users?doctor-id=" + doctor.EMail);
                JObject json = JObject.Parse(jsonResult);
                JArray jsonArray = (JArray)json.GetValue("users");

                foreach (JObject obj in jsonArray)
                    doctor.Patients.Add(JsonConvert.DeserializeObject<Patient>(obj.ToString()));

                //Ritorna la view della cartella home (ovvero la cartella che ha lo stesso prefisso di questo controller)
                //di nome Index, ovvero la view che ha lo stesso nome della action del controller
                return View(doctor);
            }
            else
                return Redirect("~/signin");
        }
    }
}