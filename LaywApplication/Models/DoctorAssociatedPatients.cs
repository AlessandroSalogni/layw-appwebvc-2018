using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Models
{
    public class DoctorAssociatedPatients
    {
        public Doctor Doctor { get; set; }

        public List<Patient> NoPatientYet { get; set; }
    }
}
