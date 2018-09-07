using LaywApplication.Configuration;
using LaywApplication.Mqtt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace LaywApplication.Controllers
{
    public class HomeController : Controller
    {
        public static IHubContext<MQTTHub> HubContext;

        private readonly DoctorAccount DoctorAccountConfig;

        public HomeController(IHubContext<MQTTHub> hubContext, DoctorAccount doctorAccountConfig)
        {
            HubContext = hubContext;
            DoctorAccountConfig = doctorAccountConfig;
        }

        [HttpGet("~/")]
        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var doctorEmail = User.Claims.FirstOrDefault(c => c.Type == DoctorAccountConfig.Email).Value;
                MQTTClient.Instance.AddTopic("server/" + doctorEmail);

                return Redirect("~/dashboard/homepage");
            }
            return Redirect("~/signin");
        }
    }
}