using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Abstract;
using LaywApplication.Controllers.Services;
using LaywApplication.Data;
using LaywApplication.Models;
using LinqToDB;
using LinqToDB.DataProvider.SQLite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers
{
    public class AdminAuthenticationController : BaseJsonController
    {
        private readonly string ConnectionString;
        private readonly DoctorController DoctorController;
        private readonly PatientController PatientCollectionController;
        
        public AdminAuthenticationController(string connectionString, ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Patient)
        {
            DoctorController = new DoctorController(IPConfig, jsonStructureConfig);
            PatientCollectionController = new PatientController(IPConfig, jsonStructureConfig);
            ConnectionString = connectionString;
        }

        [HttpGet]
        public IActionResult Index() => Redirect("~/signin");

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection collection)
        {
            string email = collection["email"];
            string password = collection["password"];

            var dbFactory = new AdminDataContextFactory( dataProvider: SQLiteTools.GetDataProvider(), connectionString: ConnectionString);

            Task<Admin> admin;
            using (var db = dbFactory.Create())
                admin = db.GetTable<Admin>().FirstOrDefaultAsync(c => c.Email.Equals(email) && c.Password.Equals(password));
            
            if (admin.Result != null)
                return View(new DoctorsPatients { Doctors = await DoctorController.Read(), Patients = await PatientCollectionController.Read()});
            else
                return Redirect("~/signin");
        }
    }
}