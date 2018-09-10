using LaywApplication.Models.Abstract;
using System;

namespace LaywApplication.Models.PatientData
{
    public class ActivitySummary
    {
        public DateTime Date { get; set; }
        public int Steps { get; set; }
        public int Floors { get; set; }
        public CaloriesCategory CaloriesCategory { get; set; }
    }

    public class ActivitySummarySteps : RealDataAbstract<int>
    {
        public int Steps {
            get { return RealData; }
            set { RealData = value; }
        }
    }

    public class ActivitySummaryCalories : RealDataAbstract<int>
    {
        public CaloriesCategory CaloriesCategory
        {
            set { RealData = value.OutCalories; }
        }
    }

    public class CaloriesCategory
    {
        public int OutCalories { get; set; }
    }
}
