using LaywApplication.Configuration;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers.PatientController
{
    [Route("~/dashboard/patients/{id}/[controller]")]
    public class ActivityController : BaseJsonReadController<Activity>
    {
        public ActivityController(ServerIP IPConfig, JsonStructure jsonStructureConfig)
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Activities) { }
    }
}