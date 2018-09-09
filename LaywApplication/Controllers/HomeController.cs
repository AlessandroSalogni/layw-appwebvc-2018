using LaywApplication.Configuration;
using LaywApplication.Mqtt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace LaywApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly MQTTClient MQTTClient;
        private readonly DoctorAccount DoctorAccountConfig;

        public HomeController(MQTTClient MQTTClient, DoctorAccount doctorAccountConfig)
        {
            this.MQTTClient = MQTTClient;
            DoctorAccountConfig = doctorAccountConfig;
        }

        [HttpGet("~/")]
        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var doctorEmail = User.Claims.FirstOrDefault(c => c.Type == DoctorAccountConfig.Email).Value;
                MQTTClient.AddTopic("server/" + doctorEmail + "/#");

                return Redirect("~/dashboard/homepage");
            }
            return Redirect("~/signin");
        }
    }
}