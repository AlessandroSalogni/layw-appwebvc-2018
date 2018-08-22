using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("~/")]
        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
                return Redirect("~/dashboard");
            return Redirect("~/signin");
        }
    }
}