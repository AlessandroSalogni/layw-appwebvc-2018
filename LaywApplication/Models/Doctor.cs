using System;
using System.Collections.Generic;

namespace LaywApplication.Models
{
    public class Doctor
    {
        public string EMail { get; set; }
        public string Name { get; set; }
        public Uri Image { get; set; }
        public List<Patient> Patients { get; set; }
    }
}
