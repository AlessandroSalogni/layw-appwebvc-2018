using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class DietController : BaseJsonController
    {
        public DietController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Diet) { }

        [HttpGet("~/dashboard/patients/{id}/[controller]/{day}")]
        public async Task<IEnumerable<MealKendo>> Read(int id, string day)
        {
            JObject jsonDiet = await APIUtils.GetAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url);
            JObject jsonDietDay = (JObject)jsonDiet[JsonDataConfig.Root].FirstOrDefault(x => x[JsonDataConfig.Key[0]].ToString() == day);
            List<Meal> meals = ((JArray)jsonDietDay[JsonDataConfig.Key[1]]).GetList<Meal>();

            List<MealKendo> mealsKendo = new List<MealKendo>();
            meals.ForEach(x => mealsKendo.Add(MealKendo.CreateFromOptionList(x)));

            return mealsKendo;
        }

        [HttpPost("~/dashboard/patients/{id}/[controller]/{day}/update")]
        public async Task<object> Update(int id, string day, [FromBody]MealKendo item)
        {
            JObject jsonMeal = JObject.Parse(JsonConvert.SerializeObject
                (Meal.CreateFromOptionKendoList(item), serializerSettings));

            var jsonMealDay = new JObject
            {
                { JsonDataConfig.Key[0] , day },
                { JsonDataConfig.Key[1] , new JArray() { jsonMeal } }
            };
            var jsonMealDays = new JArray
            {
                jsonMealDay
            };
            var jsonDiet = new JObject
            {
                { JsonDataConfig.Root , jsonMealDays }
            };

            await APIUtils.PostAsync(IPConfig.GetTotalUrlUser() + id + JsonDataConfig.Url, jsonDiet.ToString());
            return Empty;
        }
    }
}