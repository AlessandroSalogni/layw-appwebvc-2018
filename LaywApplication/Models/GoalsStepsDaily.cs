using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Models
{
    public class GoalsStepsDaily
    {
        [JsonIgnore]
        public DateTime Date { get; set; }
        public int Goal { get; set; }
    }
}
