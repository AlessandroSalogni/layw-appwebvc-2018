using LaywApplication.Models.Abstract;

namespace LaywApplication.Models
{
    public class Training : OptionList<Training, TrainingKendo> { }

    public class TrainingKendo : OptionKendoList<TrainingKendo, Training> { }
}
