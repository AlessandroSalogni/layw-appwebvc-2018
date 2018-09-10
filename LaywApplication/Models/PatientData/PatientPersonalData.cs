using System;

namespace LaywApplication.Models.PatientData
{
    public class PatientPersonalData
    {
        public Patient Patient { get; set; }
        public TotalSteps TotalSteps { get; } = new TotalSteps();
        public TotalCalories TotalCalories { get; } = new TotalCalories();
        public AvarageFloor AverageFloors { get; } = new AvarageFloor();
        public WeightComparison WeightComparison { get; } = new WeightComparison();
    }

    public abstract class MonthWeek<T>
    {
        public T Month { get; set; }
        public T Week { get; set; }
    }

    public class TotalSteps : MonthWeek<int> { }
    public class TotalCalories : MonthWeek<int> { }
    public class AvarageFloor : MonthWeek<double> { }

    public class WeightComparison
    {
        public PatientWeight Yesterday { get; set; }
        public PatientWeight Today { get; set; }

        public double GetPercentDifference()
        {
            return Math.Round((Today.Weight - Yesterday.Weight) * 100f / Yesterday.Weight, 2);
        }
    }
}
