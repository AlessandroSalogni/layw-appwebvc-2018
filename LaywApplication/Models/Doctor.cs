using System;
using System.Collections.Generic;
using System.Linq;

namespace LaywApplication.Models
{
    public class Doctor
    {
        public Doctor(string EMail, string Name, Uri Image)
        {
            this.EMail = EMail;
            this.Name = Name;
            this.Image = Image;
            Patients = new List<Patient>();
        }
        public string EMail { get; }
        public string Name { get; }
        public Uri Image { get; }
        public List<Patient> Patients { get; set; } //todo creo un'altra lista nel controller!!!!
    }
}
