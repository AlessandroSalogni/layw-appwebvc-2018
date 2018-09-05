using LaywApplication.Models.Abstract;
using System;

namespace LaywApplication.Models
{
    public class GoalWeight : GoalAbstract<double>
    {
        public DateTime StartDate { get { return Date; } set { Date = value; } }
        public double StartWeight { get; set; }
    }

    public class GoalStepsDaily : GoalAbstract<int> { }

    public class GoalCaloriesOut : GoalAbstract<int> { }
}
