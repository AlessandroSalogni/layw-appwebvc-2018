using LaywApplication.Models.Abstract;
using System.Collections.Generic;

namespace LaywApplication.Models
{
    public class Training : OptionList<Training, TrainingKendo>
    {
        public string DayName
        {
            get { return Name; }
            set { Name = value; }
        }

        public List<Alternative> Trainings
        {
            get { return Options; }
            set { Options = value; }
        }
    }

    public class TrainingKendo : OptionKendoList<TrainingKendo, Training>
    {
        public string DayName
        {
            get { return Name; }
            set { Name = value; }
        }
    }
}
