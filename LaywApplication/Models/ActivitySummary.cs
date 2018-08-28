using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Models
{
    public class ActivitySummary
    {
        public DateTime Date { get; set; }
        public int Steps { get; set; }
        public int Floors { get; set; }
        public CaloriesCategory CaloriesCategory { get; set; }
    }
}
