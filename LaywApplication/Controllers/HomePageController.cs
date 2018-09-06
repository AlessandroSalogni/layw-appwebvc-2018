using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
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
                var email = User.Claims.FirstOrDefault(c => c.Type == DoctorAccount.Email).Value;
                var name = User.Claims.FirstOrDefault(c => c.Type == DoctorAccount.Name).Value;
                var image = new Uri(User.Claims.FirstOrDefault(c => c.Type == DoctorAccount.ImageUri).Value);

                Models.Doctor doctor = DoctorController.Read().FirstOrDefault(x => x.Email == email);

                if (doctor == null)
                {
                    doctor = new Models.Doctor
                    {
                        Email = email,
                        Name = name,
                        Image = image,
                        Patients = new List<Patient>()
                    };

                    await DoctorController.Create(doctor);
                }
                else
                {
                    doctor.Image = image;
                    doctor.Patients = await DoctorController.Read(email);
                }

                HttpContext.Session.Set(sessionKeyName + email, doctor);
                return View(doctor);
            }
            else
                return Redirect("~/signin");
        }
    }
}