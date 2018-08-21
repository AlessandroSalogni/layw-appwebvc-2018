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
            Patients = new Patients();
        }
        public string EMail { get; }
        public string Name { get; }
        public Uri Image { get; }
        public Patients Patients { get; set; }
    }

    public class Patients : List<Patient>
    {
        public new Patient this[int index] => this.FirstOrDefault(x => x.Id == index);
    }
}
