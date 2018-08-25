using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class TrainingController : Controller
    {
        private static readonly object Empty = new { };
        private readonly IOptions<ServerIP> config;

        public TrainingController(IOptions<ServerIP> config)
        {
            this.config = config;
        }

        [HttpGet("~/dashboard/training")]
        public IEnumerable<Training> Read()
        {//todo passare id in qualche modo
            var jsonTraining = APIUtils.Get(config.Value.GetTotalUrlUser() + "1/trainings"); //todo mettere path nel config
            JArray jsonTrainingDayArray = (JArray)jsonTraining.GetValue("training-days");
            List<Training> trainingList = new List<Training>();

            foreach (JObject jsonTrainingDay in jsonTrainingDayArray)
            {
                Training training = new Training { DayName = (string)jsonTrainingDay.GetValue("dayName") };
                JArray jsonTrainingExerciseArray = (JArray)jsonTrainingDay.GetValue("trainings");

                if(jsonTrainingExerciseArray != null)
                {
                    int i = 0;
                    foreach (JObject jsonTrainingExercise in jsonTrainingExerciseArray)
                    {
                        var description = (string)jsonTrainingExercise.GetValue("description");
                        if (i == 0)
                            training.Exercise1 = description;
                        else if (i == 1)
                            training.Exercise2 = description;
                        else if (i == 2)
                            training.Exercise3 = description;
                        else if (i >= 3)
                            break;

                        i++;
                    }
                }

                trainingList.Add(training);
            }

            return trainingList;
        }
        
        [HttpPost("~/dashboard/training/create")]
        public object Create([FromBody]Training item)
        {
            return Empty;
        }

        [HttpPost("~/dashboard/training/update")]
        public object Update([FromBody]Training item)
        {
            return Empty;
        }

        [HttpPost("~/dashboard/training/delete")]
        public void Delete([FromBody]Training item)
        {
        }
    }
}