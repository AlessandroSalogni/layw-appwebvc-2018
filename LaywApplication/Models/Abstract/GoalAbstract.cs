using System;

namespace LaywApplication.Models.Abstract
{
    public abstract class GoalAbstract<TType>
    {
        public DateTime Date { get; set; }
        public TType Goal { get; set; }
    }
}
