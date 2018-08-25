using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Models
{
    public class Training
    {
        public string DayName { get; set; }
        public List<Exercise> Trainings { get; set; }
        public string Exercise1
        {
            get
            {
                return getExerciseDescription(0);
            }
        }
        public string Exercise2
        {
            get
            {
                return getExerciseDescription(1);
            }
        }
        public string Exercise3
        {
            get
            {
                return getExerciseDescription(2);
            }
        }

        private string getExerciseDescription(int indexExercise)
        {
            return Trainings?.ElementAtOrDefault(indexExercise)?.Description;
        }

        public class Exercise
        {
            public string Description { get; set; }
        }
    }
}
