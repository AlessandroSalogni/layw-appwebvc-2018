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
            //Cercare di memorizzare la mail del dottore per utilizzarla ovunque
            string jsonResult = Utils.Get("http://localhost:4567/api/v1.0/users?doctor-id=" + User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value);
            JObject json = JObject.Parse(jsonResult);
            JArray jsonArray = (JArray)json.GetValue("users");

            List<Patient> patients = new List<Patient>();

            foreach (JObject obj in jsonArray)
                patients.Add(JsonConvert.DeserializeObject<Patient>(obj.ToString()));


            //Ritorna la view della cartella home (ovvero la cartella che ha lo stesso prefisso di questo controller)
            //di nome Index, ovvero la view che ha lo stesso nome della action del controller
            return View(patients);
        }
    }
}