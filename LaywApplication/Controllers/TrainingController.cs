using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers
{
    public class TrainingController : Controller
    {
        static readonly object Empty = new { };

        [HttpGet("~/dashboard/training")]
        public IEnumerable<Training> Read()
        {
            List<Training> trainingList = new List<Training>();

            trainingList.Add(new Training { Day = "Lunedì" });
            trainingList.Add(new Training { Day = "Martedì" });
            trainingList.Add(new Training { Day = "Mercoledì" });
            trainingList.Add(new Training { Day = "Giovedì" });
            trainingList.Add(new Training { Day = "Venerdì" });
            trainingList.Add(new Training { Day = "Sabato" });
            trainingList.Add(new Training { Day = "Domenica" });

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