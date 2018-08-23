using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Models
{
    public class Activity
    {
        List<HeartBeat> HeartBeats { get; set; }
        string ActivityName { get; set; }
        DateTime Date { get; set; }
    }

    internal class HeartBeat
    {
        DateTime HeartRateTime { get; set; }
        int Value { get; set; }
    }
}   
