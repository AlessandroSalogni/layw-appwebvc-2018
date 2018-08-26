using System.Collections.Generic;
using System.Linq;

namespace LaywApplication.Models.Abstract
{
    public abstract class OptionList<TFirst, TSecond>
        where TFirst : OptionList<TFirst, TSecond>, new()
        where TSecond : OptionKendoList<TSecond, TFirst>, new()
    {
        public string Name { get; set; }
        public List<Alternative> Options { get; set; }

        public static TFirst CreateFromOptionKendoList(OptionKendoList<TSecond, TFirst> optionKendoList) =>
            new TFirst
            {
                Name = optionKendoList.Name,
                Options = GetAlternativeList(optionKendoList)
            };

        private static List<Alternative> GetAlternativeList(OptionKendoList<TSecond, TFirst> optionKendoList)
        {
            List<Alternative> options = new List<Alternative>();

            if (HasDescription(optionKendoList.Option1))
                options.Add(new Alternative { Description = optionKendoList.Option1 });
            if (HasDescription(optionKendoList.Option2))
                options.Add(new Alternative { Description = optionKendoList.Option2 });
            if (HasDescription(optionKendoList.Option3))
                options.Add(new Alternative { Description = optionKendoList.Option3 });

            return options;
        }

        private static bool HasDescription(string description) => description != null && description != "";


        public class Alternative
        {
            public string Description { get; set; }
        }
    }

    public abstract class OptionKendoList<TFirst, TSecond>
        where TFirst : OptionKendoList<TFirst, TSecond>, new()
        where TSecond : OptionList<TSecond, TFirst>, new()
    {
        public string Name { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }

        public static TFirst CreateFromOptionList(OptionList<TSecond, TFirst> optionList) =>
            new TFirst
            {
                Name = optionList.Name,
                Option1 = GetAlternativeDescription(optionList, 0),
                Option2 = GetAlternativeDescription(optionList, 1),
                Option3 = GetAlternativeDescription(optionList, 2)
            };

        private static string GetAlternativeDescription(OptionList<TSecond, TFirst> optionList, int indexExercise) =>
            optionList.Options?.ElementAtOrDefault(indexExercise)?.Description;
    }
}
