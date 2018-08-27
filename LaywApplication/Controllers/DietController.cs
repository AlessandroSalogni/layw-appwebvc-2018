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
using Newtonsoft.Json.Serialization;

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

        [HttpGet("~/dashboard/patients/{id}/[controller]/{day}")]
        public async Task<IEnumerable<MealKendo>> Read(int id, string day)
        {
            JObject jsonDiet = await APIUtils.GetAsync(config.Value.GetTotalUrlUser() + id + "/diets"); //todo mettere path nel config
            JObject jsonDietDay = (JObject)jsonDiet["diet-days"].FirstOrDefault(x => x["dayName"].ToString() == day);
            List<Meal> meals = ((JArray)jsonDietDay["meals"]).GetList<Meal>();

            List<MealKendo> mealsKendo = new List<MealKendo>();
            meals.ForEach(x => mealsKendo.Add(MealKendo.CreateFromOptionList(x)));

            return mealsKendo;
        }

        [HttpPost("~/dashboard/patients/{id}/[controller]/{day}/update")]
        public async Task<object> Update(int id, string day, [FromBody]MealKendo item)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            Meal meal = Meal.CreateFromOptionKendoList(item);
            JObject jsonMeal = JObject.Parse(JsonConvert.SerializeObject(meal, serializerSettings));

            var jsonMealDay = new JObject
            {
                { "dayName", day },
                { "meals", new JArray() { jsonMeal } }
            };
            var jsonMealDays = new JArray
            {
                jsonMealDay
            };
            var jsonDiet = new JObject
            {
                { "diet-days", jsonMealDays }
            };

            await APIUtils.PostAsync(config.Value.GetTotalUrlUser() + id + "/diets", jsonDiet.ToString());

            return Empty;
        }
    }
}