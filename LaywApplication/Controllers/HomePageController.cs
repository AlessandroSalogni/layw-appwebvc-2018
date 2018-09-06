using System;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers
{
    public class HomePageController : BaseController
    {
        private readonly DoctorAccount DoctorAccount;
        private readonly DoctorController DoctorController;

        public HomePageController(ServerIP IPConfig, JsonStructure jsonStructure, DoctorAccount doctorAccount) 
            : base(IPConfig)
        {
            DoctorAccount = doctorAccount;
            DoctorController = new DoctorController(IPConfig, jsonStructure);
        }

        [HttpGet("~/dashboard/[controller]")]
        public async Task<IActionResult> Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var doctor = new Models.Doctor
                {
                    Email = User.Claims.FirstOrDefault(c => c.Type == DoctorAccount.Email).Value,
                    Name = User.Claims.FirstOrDefault(c => c.Type == DoctorAccount.Name).Value,
                    Image = new Uri(User.Claims.FirstOrDefault(c => c.Type == DoctorAccount.ImageUri).Value),
                };
                doctor.Patients = await DoctorController.Read(doctor.Email);
                await DoctorController.Create(doctor);

                HttpContext.Session.Set(sessionKeyName + doctor.Email, doctor);
                return View(doctor);
            }
            else
                return Redirect("~/signin");
        }
    }
}