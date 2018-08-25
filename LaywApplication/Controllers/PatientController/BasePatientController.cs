using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public abstract class BasePatientController : Controller
    {
        protected Configuration.Parameters ParametersConfig { get; set; }
        protected readonly ServerIP IPconfig;

        protected readonly IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };
        
        public BasePatientController(IOptions<ServerIP> IPconfig, IOptions<JsonStructure> parameters)
        {
            this.IPconfig = IPconfig.Value;
            ParametersConfig = parameters.Value.Parameters;
        }
    }
}