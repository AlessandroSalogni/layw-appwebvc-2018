using LaywApplication.Configuration;
using LaywApplication.Controllers.Services.PatientData.Abstract;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers.Services.PatientData
{
  
    [Route("~/dashboard/patients/{id}/[controller]")]
    public class GoalStepsDailyStatisticsController 
        : GoalStatisticsController<int, GoalStepsDaily, ActivitySummarySteps>
    {
        public GoalStepsDailyStatisticsController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, 
                  new GoalStepsDailyController(IPConfig, jsonStructureConfig),
                  new ActivitySummaryStepsController(IPConfig, jsonStructureConfig)) { }
    }

    [Route("~/dashboard/patients/{id}/[controller]")]
    public class GoalWeightStatisticsController : GoalStatisticsController<double, GoalWeight, PatientWeight>
    {
        public GoalWeightStatisticsController(ServerIP IPConfig, JsonStructure jsonStructureConfig)
            : base(IPConfig, jsonStructureConfig,
                  new GoalWeightController(IPConfig, jsonStructureConfig),
                  new WeightController(IPConfig, jsonStructureConfig)) { }
    }

    [Route("~/dashboard/patients/{id}/[controller]")]
    public class GoalCaloriesOutStatisticsController 
        : GoalStatisticsController<int, Models.GoalCaloriesOut, ActivitySummaryCalories>
    {
        public GoalCaloriesOutStatisticsController(ServerIP IPConfig, JsonStructure jsonStructureConfig)
            : base(IPConfig, jsonStructureConfig,
                  new GoalCaloriesOutController(IPConfig, jsonStructureConfig),
                  new ActivitySummaryCaloriesController(IPConfig, jsonStructureConfig)) { }
    }
}