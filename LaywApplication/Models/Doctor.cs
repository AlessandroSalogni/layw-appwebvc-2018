using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LaywApplication.Models
{
    public class Doctor
    {
        public string Email { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public Uri Image { get; set; }

        [JsonIgnore]
        public List<Patient> Patients { get; set; }
    }
}
