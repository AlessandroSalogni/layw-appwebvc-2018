using System;

namespace LaywApplication.Models
{
    public enum Gender {MALE, FEMALE}
    public class Patient
    {
        public String Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public int Height { get; set; }
        public int Age { get; set; }
        public String Country { get; set; }
        public int Id { get; set; }
    }
}
