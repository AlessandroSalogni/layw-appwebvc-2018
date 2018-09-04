using Newtonsoft.Json;
using System;

namespace LaywApplication.Models
{
    public class GoalsWeight
    {
        [JsonIgnore]
        public DateTime Date { get; set; }
        public DateTime StartDate { get { return Date; } set { Date = value; } }
        public double Goal { get; set; }
        public double StartWeight { get; set; }
    }
}
