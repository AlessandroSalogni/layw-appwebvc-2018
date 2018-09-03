using LaywApplication.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;

namespace LaywApplication.Controllers
{
    public class BaseController : Controller
    {
        protected static readonly IsoDateTimeConverter italianDateConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };
        protected static readonly object Empty = new { };

        protected readonly ServerIP IPConfig;

        protected BaseController(ServerIP IPConfig) => this.IPConfig = IPConfig;
    }
}