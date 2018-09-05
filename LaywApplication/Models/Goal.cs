using Newtonsoft.Json;
using System;

namespace LaywApplication.Models
{
    public abstract class GoalAbstract<TType>
    {
        [JsonIgnore]
        public DateTime Date { get; set; }
        public TType Goal { get; set; }
    }

    public class Goal : GoalAbstract<double>
    {
        public DateTime StartDate { get { return Date; } set { Date = value; } }
        public double StartWeight { get; set; }
    }

    public class GoalStepsDaily : GoalAbstract<int> { }

    public class GoalCaloriesOut : GoalAbstract<int> { }
}
