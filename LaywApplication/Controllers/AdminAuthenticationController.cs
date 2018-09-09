using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Abstract;
using LaywApplication.Controllers.Services;
using LaywApplication.Controllers.Utils;
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
        private MailData MailData { get; set; }

        public AdminAuthenticationController(string connectionString, ServerIP IPConfig, JsonStructure jsonStructureConfig, MailData mailData) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Patient)
        {
            DoctorController = new DoctorController(IPConfig, jsonStructureConfig);
            PatientCollectionController = new PatientController(IPConfig, jsonStructureConfig);
            ConnectionString = connectionString;
            MailData = mailData;
        }

        [HttpGet]
        public IActionResult Index() => Redirect("~/signin");

        [HttpGet("read")]
        public List<Admin> Read()
        {
            var dbFactory = new AdminDataContextFactory(dataProvider: SQLiteTools.GetDataProvider(), connectionString: ConnectionString);

            List<Admin> admins = new List<Admin>();
            using (var db = dbFactory.Create())
            {
                admins = db.GetTable<Admin>().ToList();
            }

            return admins;
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection collection)
        {
            string email = collection["email"];
            string password = PasswordUtils.MD5Crypt(collection["password"]);

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
        
        [HttpPut("{email}/update")]
        public object Update(string email, IFormCollection collection)
        {
            string oldPassword = PasswordUtils.MD5Crypt(collection["oldPassword"]);
            string newPassword = PasswordUtils.MD5Crypt(collection["newPassword"]);
            
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
                    return "Old password wrong, or an error occurred.";
            }
        }

        [HttpPost("create")]
        public object Create(IFormCollection collection)
        {
            string email = collection["mailNewAdmin"];
            string passwordNotCrypted = PasswordUtils.PasswordGenerator();
            string password = PasswordUtils.MD5Crypt(passwordNotCrypted);
            var dbFactory = new AdminDataContextFactory(dataProvider: SQLiteTools.GetDataProvider(), connectionString: ConnectionString);
            
            using (var db = dbFactory.Create())
            {
                db.InsertOrReplace(new Admin { Email = email, Password = password });

                try
                { 
                    (new Emailer(MailData)).SendEmail(email, "Congratulations! You have just become a layw administrator. Your login data:\n" +
                    "Mail: " + email + "\n" + "Password: " + passwordNotCrypted + "\n" +
                    "Remember to change your password. Thank you for your help.\n\nTeam LAYW", "Team LAYW");
                }
                catch (SmtpException)
                {
                    db.Delete(new Admin { Email = email, Password = password });
                    return "Error. Maybe the inserted email is invalid, or there are connection errors. Try again.";
                }
                catch (FormatException)
                {
                    db.Delete(new Admin { Email = email, Password = password });
                    return "Invalid email format";
                }
            }
            return "Successfully added";
        }
    }
}