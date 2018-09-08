using System;
using System.Linq;
using System.Net.Mail;
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


        [HttpPut("{email}/update")]
        public object Update(string email, IFormCollection collection)
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

        [HttpPost("create")]
        public object Create(IFormCollection collection)
        {
            string email = collection["mailNewAdmin"];
            string password = PasswordGenerator();
            var dbFactory = new AdminDataContextFactory(dataProvider: SQLiteTools.GetDataProvider(), connectionString: ConnectionString);
            
            using (var db = dbFactory.Create())
            {
                db.InsertOrReplace(new Admin { Email = email, Password = password });

                SendEmail(email, password);

                db.Delete(new Admin { Email = email, Password = password});
            }

            return Empty;
        }

        private string PasswordGenerator()
        {
            Random random = new Random();
            string chars = "abcdefghilmnopqrstuvz0123456789";
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void SendEmail(string email, string password)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("lookafteryourweight@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Layw Admin";
                mail.Body = "Congratulations! You have just become a layw administrator. Your login data:\n" +
                    "Mail: " + email + "\n" + "Password: " + password + ".\n" +
                    "Remember to change your password. Thank you for your help.\n\nTeam LAYW";
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("lookafteryourweight@gmail.com", "layw-2018");
                SmtpServer.EnableSsl = true;
                SmtpServer.Timeout = 20000;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }
        }
    }
}