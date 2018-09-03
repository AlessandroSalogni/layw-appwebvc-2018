using System.Collections.Generic;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class TrainingController : BaseJsonController
    {
        public TrainingController(ServerIP IPConfig, JsonStructure jsonStructure)
            : base(IPConfig, jsonStructure, jsonStructure.Training) { }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<IEnumerable<TrainingKendo>> Read(int id)
        {   
            JObject jsonTraining = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url);
            List<Models.Training> trainings = ((JArray)jsonTraining[JsonDataConfig.Root]).GetList<Models.Training>();

            List<TrainingKendo> trainingsKendo = new List<TrainingKendo>();
            trainings.ForEach(x => trainingsKendo.Add(TrainingKendo.CreateFromOptionList(x)));

            return trainingsKendo;
        }

        [HttpPost("~/dashboard/patients/{id}/[controller]/update")]
        public async Task<object> Update(int id, [FromBody]TrainingKendo item)
        {
            JObject jsonDayTrainings = JObject.Parse(JsonConvert.SerializeObject
                (Models.Training.CreateFromOptionKendoList(item), serializerSettings));

            var jsonTrainings = new JObject
            {
                { JsonDataConfig.Root, new JArray() { jsonDayTrainings } }
            };

            await APIUtils.PostAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url, jsonTrainings.ToString());
            return Empty;
        }
    }
}