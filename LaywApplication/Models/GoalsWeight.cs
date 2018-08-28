using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
