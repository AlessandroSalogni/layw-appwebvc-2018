﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<IEnumerable<TrainingKendo>> Read(int id)
        {   
            JObject jsonTraining = await APIUtils.GetAsync(config.Value.GetTotalUrlUser() + id + "/trainings"); //todo mettere path nel config
            List<Training> trainings = ((JArray)jsonTraining["training-days"]).GetList<Training>();

            List<TrainingKendo> trainingsKendo = new List<TrainingKendo>();
            trainings.ForEach(x => trainingsKendo.Add(TrainingKendo.CreateFromOptionList(x)));

            return trainingsKendo;
        }

        [HttpPost("~/dashboard/patients/{id}/[controller]/update")]
        public async Task<object> Update(int id, [FromBody]TrainingKendo item)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            Training training = Training.CreateFromOptionKendoList(item);
            JObject jsonDayTrainings = JObject.Parse(JsonConvert.SerializeObject(training, serializerSettings));

            var jsonTrainings = new JObject
            {
                { "training-days", new JArray() { jsonDayTrainings } }
            };

            await APIUtils.PostAsync(config.Value.GetTotalUrlUser() + id + "/trainings", jsonTrainings.ToString());

            return Empty;
        }
    }
}