using LaywApplication.Models.Abstract;

namespace LaywApplication.Models
{
    public class Meal : OptionList<Meal, MealKendo> { }

    public class MealKendo : OptionKendoList<MealKendo, Meal> { }
}
