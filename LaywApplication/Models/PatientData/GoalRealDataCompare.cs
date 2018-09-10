namespace LaywApplication.Models.PatientData
{
    public class GoalRealDataCompare<TType>
    {
        public TType RealData { get; set; }
        public TType Goal { get; set; }
        public string Date { get; set; }
    }
}
