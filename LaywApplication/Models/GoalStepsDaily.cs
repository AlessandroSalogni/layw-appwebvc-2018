using Newtonsoft.Json;
using System;

namespace LaywApplication.Models
{
    public class GoalStepsDaily
    {
        [JsonIgnore]
        public DateTime Date { get; set; }
        public int Goal { get; set; }
    }
}
