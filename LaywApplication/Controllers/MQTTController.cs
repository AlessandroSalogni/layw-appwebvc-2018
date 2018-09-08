using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers
{
    public class MQTTController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}