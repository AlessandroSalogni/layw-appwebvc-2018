using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Models
{
    public class PatientPersonalData
    {
        public int TotalSteps { get; set; }
        public int TotalCalories { get; set; }
        public Weights[] LastTwoWeights { get; set; }
        public Double AverageFloors { get; set; }
    }
}
