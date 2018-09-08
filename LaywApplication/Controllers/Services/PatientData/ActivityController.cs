using LaywApplication.Configuration;
using LaywApplication.Controllers.Services.PatientData.Abstract;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers.Services.PatientData
{
    [Route("~/dashboard/patients/{id}/[controller]")]
    public class ActivityController : BaseJsonReadController<Activity>
    {
        public ActivityController(ServerIP IPConfig, JsonStructure jsonStructureConfig)
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Activities) { }
    }
}