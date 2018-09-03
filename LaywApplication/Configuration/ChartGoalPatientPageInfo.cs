namespace LaywApplication.Configuration
{
    public class ChartGoalPatientPageInfo
    {
        public WeightChartInfo WeightChartInfo { get; set; }
        public StepsDailyChartInfo StepsDailyChartInfo { get; set; }
        public CaloriesOutChartInfo CaloriesOutChartInfo { get; set; }
    }

    public class ChartInfo
    {
        public string Title { get; set; }
        public string Controller { get; set; }
        public string ControllerGoal { get; set; }
        public string[] LegendTitle { get; set; }
        public int AxisXInterval { get; set; }
    }

    public class WeightChartInfo : ChartInfo { }
    public class StepsDailyChartInfo : ChartInfo { }
    public class CaloriesOutChartInfo : ChartInfo { }
}
