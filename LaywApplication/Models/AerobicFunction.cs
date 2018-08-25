using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Models
{
    public static class AerobicFunction
    {
        const int MALE_UPPER_VALUE = 220;
        const int FEMALE_UPPER_VALUE = 206;

        public static (int, int) CalculateRange(Patient patient)
        {
            if (patient.Gender == Gender.FEMALE)
                return ((FEMALE_UPPER_VALUE - patient.Age)*65/100, (FEMALE_UPPER_VALUE - patient.Age) * 85 / 100);
            return ((MALE_UPPER_VALUE - patient.Age) * 65 / 100, (MALE_UPPER_VALUE - patient.Age) * 85 / 100);
        }
    }
}
