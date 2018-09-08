using LaywApplication.Configuration;
using LaywApplication.Controllers.Abstract;
using LaywApplication.Controllers.Services.PatientData;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers.Services
{
    [Route("~/dashboard/homepage/[controller]")]
    public class GoalStepsDailyStatisticsHomepageController
      : GoalStatisticsHomepageAbstractController<int, GoalStepsDaily, ActivitySummarySteps>
    {
        public GoalStepsDailyStatisticsHomepageController(ServerIP IPConfig, JsonStructure jsonStructureConfig,
            ChartHomepageInfo homepageChartInfo)
            : base(IPConfig, jsonStructureConfig, homepageChartInfo,
                  new GoalStepsDailyController(IPConfig, jsonStructureConfig),
                  new ActivitySummaryStepsController(IPConfig, jsonStructureConfig)) { }

        protected override bool GoalIsAchieved(int goal, int realData) => goal.CompareTo(realData) <= 0;
    }

    [Route("~/dashboard/homepage/[controller]")]
    public class GoalWeightStatisticsHomepageController
        : GoalStatisticsHomepageAbstractController<double, GoalWeight, PatientWeight>
    {
        public GoalWeightStatisticsHomepageController(ServerIP IPConfig, JsonStructure jsonStructureConfig,
            ChartHomepageInfo homepageChartInfo)
            : base(IPConfig, jsonStructureConfig, homepageChartInfo,
                  new GoalWeightController(IPConfig, jsonStructureConfig),
                  new WeightController(IPConfig, jsonStructureConfig)) { }

        protected override bool GoalIsAchieved(double goal, double realData) => goal.CompareTo(realData) >= 0;
    }

    [Route("~/dashboard/homepage/[controller]")]
    public class GoalCaloriesStatisticsHomepageController 
        : GoalStatisticsHomepageAbstractController<int, Models.GoalCaloriesOut, ActivitySummaryCalories>
    {
        public GoalCaloriesStatisticsHomepageController(ServerIP IPConfig, JsonStructure jsonStructureConfig,
            ChartHomepageInfo homepageChartInfo)
            : base(IPConfig, jsonStructureConfig, homepageChartInfo,
                  new GoalCaloriesOutController(IPConfig, jsonStructureConfig),
                  new ActivitySummaryCaloriesController(IPConfig, jsonStructureConfig)) { }

        protected override bool GoalIsAchieved(int goal, int realData) => goal.CompareTo(realData) <= 0;
    }
}