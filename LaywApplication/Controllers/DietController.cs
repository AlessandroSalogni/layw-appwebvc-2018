using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers
{
    public class DietController : Controller
    {
        static readonly object Empty = new { };

        [HttpGet("~/dashboard/diet")]
        public IEnumerable<Diet> Read()
        {
            List<Diet> dietList = new List<Diet>();

            dietList.Add(new Diet { Day = "Lunedì"});
            dietList.Add(new Diet { Day = "Martedì" });
            dietList.Add(new Diet { Day = "Mercoledì" });
            dietList.Add(new Diet { Day = "Giovedì" });
            dietList.Add(new Diet { Day = "Venerdì" });
            dietList.Add(new Diet { Day = "Sabato" });
            dietList.Add(new Diet { Day = "Domenica" });

            return dietList;
        }

        [HttpPost("~/dashboard/diet/create")]
        public object Create([FromBody]Diet item)
        {
            return Empty;
        }

        [HttpPost("~/dashboard/diet/update")]
        public object Update([FromBody]Diet item)
        {
            return Empty;
        }

        [HttpPost("~/dashboard/diet/delete")]
        public void Delete([FromBody]Diet item)
        {
        }
    }
}