using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            String jsonResult = Get("http://localhost:4567/api/v1.0/users/2");
            JObject json = JObject.Parse(jsonResult);
            
            Patient p = JsonConvert.DeserializeObject<Patient>(json.GetValue("user").ToString());
            
            //Ritorna la view della cartella home (ovvero la cartella che ha lo stesso prefisso di questo controller)
            //di nome Index, ovvero la view che ha lo stesso nome della action del controller
            return View(p);
        }

        public string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                            return reader.ReadToEnd();
            }
            catch(WebException e) { }

            return "{}";
        }
    }
}