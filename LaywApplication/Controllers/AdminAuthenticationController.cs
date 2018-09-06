using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Data;
using LaywApplication.Extensions;
using LaywApplication.Models;
using LinqToDB;
using LinqToDB.DataProvider.SQLite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers
{
    public class AdminAuthenticationController : BaseJsonController
    {
        private string ConnectionString { get; set; }
        private DoctorController doctorController;
        private PatientCollectionController patientCollectionController;
        

        public AdminAuthenticationController(string connectionString, ServerIP IPConfig, JsonStructure jsonStructureConfig) : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Patient)
        {
            doctorController = new DoctorController(IPConfig, jsonStructureConfig);
            patientCollectionController = new PatientCollectionController(IPConfig, jsonStructureConfig);
            ConnectionString = connectionString;
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection collection)
        {
            string email = collection["email"];
            string password = collection["password"];

            var dbFactory = new AdminDataContextFactory(
                dataProvider: SQLiteTools.GetDataProvider(),
                connectionString: ConnectionString
            );

            Task<Admin> admin;

            using (var db = dbFactory.Create())
            {
                admin = db.GetTable<Admin>().FirstOrDefaultAsync(c => c.Email.Equals(email) && c.Password.Equals(password));
            }

            if (admin.Result != null)
                return View(await GetDoctorAssociatedPatients());
            else
                return Redirect("~/signin");
        }

        private async Task<List<DoctorAssociatedPatients>> GetDoctorAssociatedPatients()
        {
            List<DoctorAssociatedPatients> doctorAssociatedPatients = new List<DoctorAssociatedPatients>();

            foreach (Models.Doctor doctor in await doctorController.Read())
            {
                List<Models.Patient> allPatients = await patientCollectionController.Read();
                IEnumerable<Models.Patient> noPatientYet = allPatients.Except(doctor.Patients);

                doctorAssociatedPatients.Add(new DoctorAssociatedPatients { Doctor = doctor, NoPatientYet = noPatientYet.ToList() });
            }

            return doctorAssociatedPatients;
        }
    }
}