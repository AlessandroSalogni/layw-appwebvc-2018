using LaywApplication.Models.Abstract;

namespace LaywApplication.Models
{
    public class PatientWeight : RealDataAbstract<double>
    {
        public double Weight {
            get { return RealData; }
            set { RealData = value; }
        }

        public double Bmi { get; set; }
    }
}
