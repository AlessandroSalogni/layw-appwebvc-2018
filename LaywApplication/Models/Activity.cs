using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Models
{
    public class Activity
    {
        public List<HeartBeat> HeartBeats { get; set; }
        public string ActivityName { get; set; }
        public DateTime Date { get; set; }
        public long Duration { get; set; }
    }

    public class HeartBeat
    {
        public string HeartRateTime { get; set; }
        public int Value { get; set; }
    }
}   
