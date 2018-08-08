using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Models
{
    public class Doctor
    {
        public Doctor(string EMail, string Name, Uri Image)
        {
            this.EMail = EMail;
            this.Name = Name;
            this.Image = Image;
            this.Patients = new List<Patient>();
        }

        public string EMail { get; }
        public string Name { get; }
        public Uri Image { get; }
        public IList<Patient> Patients { get; set; }
    }
}
