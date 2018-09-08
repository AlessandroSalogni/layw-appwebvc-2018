using LaywApplication.Configuration;
using LaywApplication.Controllers.Services.PatientData.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers.Services.PatientData
{
    [Route("~/dashboard/patients/{id}/[controller]")]
    public class ActivitySummaryController : BaseJsonReadController<Models.ActivitySummary>
    {
        public ActivitySummaryController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.ActivitySummary) { }
    }

    [Route("~/dashboard/patients/{id}/[controller]")]
    public class ActivitySummaryStepsController : BaseJsonReadController<Models.ActivitySummarySteps>
    {
        public ActivitySummaryStepsController(ServerIP IPConfig, JsonStructure jsonStructureConfig)
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.ActivitySummary) { }
    }

    [Route("~/dashboard/patients/{id}/[controller]")]
    public class ActivitySummaryCaloriesController : BaseJsonReadController<Models.ActivitySummaryCalories>
    {
        public ActivitySummaryCaloriesController(ServerIP IPConfig, JsonStructure jsonStructureConfig)
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.ActivitySummary) { }
    }
}