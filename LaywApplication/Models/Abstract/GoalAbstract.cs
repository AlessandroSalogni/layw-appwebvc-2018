using System;

namespace LaywApplication.Models.Abstract
{
    public abstract class GoalAbstract<TType> where TType : struct
    {
        public DateTime Date { get; set; }
        public TType Goal { get; set; }
    }
}
