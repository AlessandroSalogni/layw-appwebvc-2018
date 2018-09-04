using LaywApplication.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LaywApplication.ViewComponents
{
    public abstract class BaseViewComponent : ViewComponent
    {
        protected static readonly string italianDateFormat = "dd-MM-yyyy";

        protected readonly ServerIP IPConfig;
        protected readonly JsonStructure JsonStructureConfig;
        protected readonly DateTime DateTimeNow = DateTime.Now;

        public BaseViewComponent(ServerIP IPConfig, JsonStructure jsonStructureConfig)
        {
            this.IPConfig = IPConfig;
            JsonStructureConfig = jsonStructureConfig;
        }
    }
}
