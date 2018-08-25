using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class DietController : Controller
    {
        private static readonly object Empty = new { };

        private readonly IOptions<ServerIP> config;
        public DietController(IOptions<ServerIP> config)
        {
            this.config = config;
        }

        [HttpGet("~/dashboard/diet")]
        public IEnumerable<Diet> Read()
        {//todo passare id in qualche modo
            var jsonDiet = APIUtils.Get(config.Value.GetTotalUrlUser() + "1/diets"); //todo mettere path nel config
            JArray jsonDietDayArray = (JArray)jsonDiet.GetValue("diet-days");
            List<Diet> dietList = new List<Diet>();

            foreach (JObject jsonDietDay in jsonDietDayArray)
            {
                Diet diet = new Diet { Day = (string)jsonDietDay.GetValue("dayName") };
                JArray jsonDietMealsArray = (JArray)jsonDietDay.GetValue("meals");

                if (jsonDietMealsArray != null)
                {
                    foreach (JObject jsonDietMeals in jsonDietMealsArray)
                    {
                        var mealName = (string)jsonDietMeals.GetValue("mealName");
                        JArray jsonDietDescription = (JArray)jsonDietMeals.GetValue("diets");
                        
                        if (jsonDietDescription != null)
                        {
                            int i = 0;
                            foreach (JObject jsonDietMealsDescription in jsonDietDescription)
                            {
                                var description = (string)jsonDietMealsDescription.GetValue("description");
                                if (i == 0)
                                    diet.Option1 = description;
                                else if (i == 1)
                                    diet.Option2 = description;
                                else if (i == 2)
                                    diet.Option3 = description;
                                else if (i >= 3)
                                    break;
                                i++;
                            }
                        }
                    }
                }

                dietList.Add(diet);
            }

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