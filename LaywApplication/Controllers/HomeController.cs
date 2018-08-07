using System;
using System.Collections.Generic;
using LaywApplication.Controllers.APIUtils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("~/")]
        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
                return Redirect("~/dashboard");
            return View();
        }
    }
}