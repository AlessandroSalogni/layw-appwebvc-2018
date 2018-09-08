using LaywApplication.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using System;

namespace LaywApplication.Controllers.Abstract
{
    public class BaseController : Controller
    {
        protected static readonly string sessionKeyName = "Doctor_";
        protected static readonly string italianDateFormat = "dd-MM-yyyy";
        protected static readonly IsoDateTimeConverter italianDateConverter = new IsoDateTimeConverter { DateTimeFormat = italianDateFormat };
        protected static readonly object Empty = new { };

        protected readonly DateTime DateTimeNow = DateTime.Now;
        protected readonly ServerIP IPConfig;

        protected BaseController(ServerIP IPConfig) => this.IPConfig = IPConfig;

        protected BaseController() { }
    }
}