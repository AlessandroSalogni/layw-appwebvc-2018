using LaywApplication.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers.PatientController
{
    [Route("~/dashboard/patients/{id}/[controller]")]
    public class ActivitySummaryController : BaseJsonReadController<Models.ActivitySummary>
    {
        public ActivitySummaryController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.ActivitySummary) { }
    }
}