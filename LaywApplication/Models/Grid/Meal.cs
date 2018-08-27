using LaywApplication.Models.Abstract;
using System.Collections.Generic;

namespace LaywApplication.Models
{
    public class Meal : OptionList<Meal, MealKendo> {
        public string MealName
        { 
            get { return Name; }
            set { Name = value; }
        }

        public List<Alternative> Diets
        {
            get { return Options; }
            set { Options = value; }
        }
    }

    public class MealKendo : OptionKendoList<MealKendo, Meal>
    {
        public string MealName
        {
            get { return Name; }
            set { Name = value; }
        }
    }
}
