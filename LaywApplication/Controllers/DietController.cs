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
            List<Diet> mealsList = new List<Diet>();

            mealsList.Add(new Diet { Meals = "Breakfast"});
            mealsList.Add(new Diet { Meals = "MorningSnack" });
            mealsList.Add(new Diet { Meals = "Lunch" });
            mealsList.Add(new Diet { Meals = "AfternoonSnack" });
            mealsList.Add(new Diet { Meals = "Dinner" });
            mealsList.Add(new Diet { Meals = "EveningSnack" });

            return mealsList;
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