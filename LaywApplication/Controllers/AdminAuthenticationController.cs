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
    [Route("~/AdminAuthentication")]
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
            {
                ViewBag.AdminEmail = email;
                return View(new DoctorsPatients { Doctors = await DoctorController.Read(), Patients = await PatientCollectionController.Read() });
            }
            else
            {
                TempData["ErrorAdminMessage"] = "Mail or password wrong";
                return Redirect("~/signin");
            }
                
        }


        [HttpPost("{email}/update")]
        public async Task<object> Update(string email, IFormCollection collection)
        {
            string oldPassword = collection["oldPassword"];
            string newPassword = collection["newPassword"];
            
            var dbFactory = new AdminDataContextFactory(dataProvider: SQLiteTools.GetDataProvider(), connectionString: ConnectionString);

            Task<Admin> admin;
            using (var db = dbFactory.Create())
            {
                admin = db.GetTable<Admin>().FirstOrDefaultAsync(c => c.Email.Equals(email) && c.Password.Equals(oldPassword));
                if (admin.Result != null)
                {
                    db.Update(new Admin { Email = email, Password = newPassword });
                    return "Password changed";
                }
                else
                    return "Old password wrong";
            }
        }
    }
}