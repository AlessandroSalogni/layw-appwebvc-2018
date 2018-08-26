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

        public static Training CreateFromTrainingKendo(TrainingKendo trainingKendo) => 
            new Training
            {
                DayName = trainingKendo.DayName,
                Trainings = GetExerciseList(trainingKendo)
            };

        private static List<Exercise> GetExerciseList(TrainingKendo trainingKendo)
        {
            List<Exercise> exercises = new List<Exercise>();

            if (HasDescription(trainingKendo.Exercise1))
                exercises.Add(new Exercise { Description = trainingKendo.Exercise1 });
            if (HasDescription(trainingKendo.Exercise2))
                exercises.Add(new Exercise { Description = trainingKendo.Exercise2 });
            if (HasDescription(trainingKendo.Exercise3))
                exercises.Add(new Exercise { Description = trainingKendo.Exercise3 });

            return exercises;
        }

        private static bool HasDescription(string description) => description != null && description != "";

        public class Exercise
        {
            public string Description { get; set; }
        }
    }

    public class TrainingKendo
    {
        public string DayName { get; set; }
        public string Exercise1 { get; set; }
        public string Exercise2 { get; set; }
        public string Exercise3 { get; set; }

        public static TrainingKendo CreateFromTraining(Training training) =>
            new TrainingKendo
            {
                DayName = training.DayName,
                Exercise1 = GetExerciseDescription(training, 0),
                Exercise2 = GetExerciseDescription(training, 1),
                Exercise3 = GetExerciseDescription(training, 2)
            };

        private static string GetExerciseDescription(Training training, int indexExercise) =>
            training.Trainings?.ElementAtOrDefault(indexExercise)?.Description;
    }
}
