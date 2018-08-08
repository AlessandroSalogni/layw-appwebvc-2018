using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Models
{
    public class Doctor
    {
        public Doctor(string EMail, string Name)
        {
            this.EMail = EMail;
            this.Name = Name;
            this.Patients = new List<Patient>();
        }

        public string EMail { get; }
        public string Name { get; }
        public IList<Patient> Patients { get; set; }
    }
}
