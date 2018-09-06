namespace LaywApplication.Configuration
{
    public class ChartGoalPatientPageInfo
    {
        public WeightPatientPageChartInfo WeightChartInfo { get; set; }
        public StepsDailyPatientPageChartInfo StepsDailyChartInfo { get; set; }
        public CaloriesOutPatientPageChartInfo CaloriesOutChartInfo { get; set; }
    }

    public class ChartPatientPageInfo : ChartInfo
    {
        public string ControllerGoal { get; set; }
        public string[] LegendTitle { get; set; }
        public int AxisXInterval { get; set; }
    }

    public class WeightPatientPageChartInfo : ChartPatientPageInfo { }
    public class StepsDailyPatientPageChartInfo : ChartPatientPageInfo { }
    public class CaloriesOutPatientPageChartInfo : ChartPatientPageInfo { }
}
