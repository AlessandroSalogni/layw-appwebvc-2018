using Newtonsoft.Json;
using System;

namespace LaywApplication.Models
{
    public enum Gender {MALE, FEMALE}
    public class Patient
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public int Height { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public int Id { get; set; }
        public double StrideLengthRunning { get; set; }
        public double StrideLengthWalking { get; set; }
        public double AverageDailySteps { get; set; }

        [JsonIgnore]
        public AerobicFunction AerobicFunction { get; set; }
    }

    public class AerobicFunction
    {
        public const int MALE_UPPER_VALUE = 220;
        public const int FEMALE_UPPER_VALUE = 206;

        public int UpLimit { get; }
        public int DownLimit { get; }

        public AerobicFunction(Patient patient)
        {
            var constValue = (patient.Gender == Gender.FEMALE) ? FEMALE_UPPER_VALUE : MALE_UPPER_VALUE;

            UpLimit = (constValue - patient.Age) * 85 / 100;
            DownLimit = (constValue - patient.Age) * 65 / 100;
        }
    }
}
