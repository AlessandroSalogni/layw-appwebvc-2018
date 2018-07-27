using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Models
{
    public enum Gender {MALE, FEMALE}
    public class Patient
    {
        public String Name { get; set; }
        public String DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public int Height { get; set; }
        public String Country { get; set; }
    }
}
