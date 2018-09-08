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
        private LaywGmailData Gmail { get; set; }

        public AdminAuthenticationController(string connectionString, ServerIP IPConfig, JsonStructure jsonStructureConfig, LaywGmailData gmail) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Patient)
        {
            DoctorController = new DoctorController(IPConfig, jsonStructureConfig);
            PatientCollectionController = new PatientController(IPConfig, jsonStructureConfig);
            ConnectionString = connectionString;
            Gmail = gmail;
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

                try
                { 
                    SendEmail(email, password);
                }
                catch (SmtpException)
                {
                    db.Delete(new Admin { Email = email, Password = password });
                    return "Error. Maybe the inserted email is invalid, or there are connection errors.";
                }
                catch (FormatException)
                {
                    db.Delete(new Admin { Email = email, Password = password });
                    return "Invalid email format";
                }
            }

            return "Successfully added";
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
                SmtpClient SmtpServer = new SmtpClient(Gmail.SMTP);

                mail.From = new MailAddress(Gmail.Email);
                mail.To.Add(email);
                mail.Subject = "LAYW Admin";
                mail.Body = "Congratulations! You have just become a layw administrator. Your login data:\n" +
                    "Mail: " + email + "\n" + "Password: " + password + ".\n" +
                    "Remember to change your password. Thank you for your help.\n\nTeam LAYW";
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Port = Gmail.Port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Gmail.Email, Gmail.Password);
                SmtpServer.EnableSsl = Gmail.EnableSSL;
                SmtpServer.Timeout = Gmail.TimeOut;
                SmtpServer.Send(mail);
            }
            catch (Exception e) when (e is SmtpException || e is FormatException)
            {
                throw e;
            }
        }
    }
}