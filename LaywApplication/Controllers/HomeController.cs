using System;
using LaywApplication.Controllers.APIUtils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string jsonResult = Utils.Get("http://localhost:4567/api/v1.0/users/1");
            JObject json = JObject.Parse(jsonResult);
            
            Patient p = JsonConvert.DeserializeObject<Patient>(json.GetValue("user").ToString());
            Utils.Post("http://localhost:4567/api/v1.0/users/1/trainings", "{\"training-days\":[{\"dayName\":\"lunedi\",\"trainings\":[{\"description\":\"20 minuti di corsa\"},{\"description\":\"30 minuti di bici\"}]},{\"dayName\":\"martedi\",\"trainings\":[{\"description\":\"1 ora camminata\"}]},{\"dayName\":\"giovedi\",\"trainings\":[{\"description\":\"1 ora Camminata\"},{\"description\":\"20 minuti di ellittica\"}]},{\"dayName\":\"venerdi\",\"trainings\":[{\"description\":\"50 addominali\"},{\"description\":\"400 addominali e 2 ore di corsa\"}]}]}");
            
            //Ritorna la view della cartella home (ovvero la cartella che ha lo stesso prefisso di questo controller)
            //di nome Index, ovvero la view che ha lo stesso nome della action del controller
            return View(p);
        }
    }
}