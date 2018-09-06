using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Configuration
{
    public class ChartHomepageInfo
    {
        public GeneralHomepageChartInfo GeneralChartInfo { get; set; }
        public WeightHomepageChartInfo WeightChartInfo { get; set; }
        public StepsDailyHomepageChartInfo StepsDailyChartInfo { get; set; }
        public CaloriesOutHomepageChartInfo CaloriesOutChartInfo { get; set; }
    }

    public class GeneralHomepageChartInfo
    {
        public string[] Legend { get; set; }
        public string[] Color { get; set; }

    }

    public class ChartInfo
    {
        public string Title { get; set; }
        public string Controller { get; set; }
    }

    public class WeightHomepageChartInfo : ChartInfo { }
    public class StepsDailyHomepageChartInfo : ChartInfo { }
    public class CaloriesOutHomepageChartInfo : ChartInfo { }
}
