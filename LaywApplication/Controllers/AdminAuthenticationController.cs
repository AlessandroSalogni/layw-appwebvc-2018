using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Data;
using LaywApplication.Extensions;
using LinqToDB;
using LinqToDB.DataProvider.SQLite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers
{
    public class AdminAuthenticationController : Controller
    {
        private string ConnectionString { get; set; }
        public AdminAuthenticationController(string connectionString)
        {
            ConnectionString = connectionString;
        }

        [HttpPost]
        public IActionResult Index(IFormCollection collection)
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
                return View();
            else
                return Redirect("~/signin");
        }
    }
}