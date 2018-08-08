using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Controllers.APIUtils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{

    public class DashboardController : Controller
    {
        [HttpGet("~/dashboard")]
        public IActionResult Index()
        {
            //todo SISTEMARE, non molto bello
            Uri apiRequestUri = new Uri(User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri").Value);
            dynamic result = JsonConvert.DeserializeObject(Utils.Get(apiRequestUri.ToString()));
            Uri image = result.picture ?? result.data.url;

            Doctor doctor = new Doctor(
                User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value, 
                User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value, 
                image);

            string jsonResult = "{\"doctor\": {\"name\": \"" + doctor.Name + "\", \"email\": \"" + doctor.EMail + "\"}}";
            Utils.Post("http://localhost:4567/api/v1.0/doctors", jsonResult);

            jsonResult = Utils.Get("http://localhost:4567/api/v1.0/users?doctor-id=" + doctor.EMail);
            JObject json = JObject.Parse(jsonResult);
            JArray jsonArray = (JArray)json.GetValue("users");

            foreach (JObject obj in jsonArray)
                doctor.Patients.Add(JsonConvert.DeserializeObject<Patient>(obj.ToString()));

            //Ritorna la view della cartella home (ovvero la cartella che ha lo stesso prefisso di questo controller)
            //di nome Index, ovvero la view che ha lo stesso nome della action del controller
            return View(doctor);
        }
    }
}