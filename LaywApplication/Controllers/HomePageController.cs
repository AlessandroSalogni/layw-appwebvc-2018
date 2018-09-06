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
        private readonly DoctorAccount DoctorAccountConfig;
        private readonly JsonStructure JsonStructureConfig;

        public HomePageController(ServerIP IPConfig, JsonStructure jsonStructureConfig, 
            DoctorAccount doctorAccountConfig) : base(IPConfig)
        {
            DoctorAccountConfig = doctorAccountConfig;
            JsonStructureConfig = jsonStructureConfig;
        }

        [HttpGet("~/dashboard/[controller]")]
        public async Task<IActionResult> Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var email = User.Claims.FirstOrDefault(c => c.Type == DoctorAccountConfig.Email).Value;
                var doctor = HttpContext.Session.Get<Models.Doctor>(sessionKeyName + email);

                if (doctor == null)
                {
                    var doctorController = new DoctorController(IPConfig, JsonStructureConfig);

                    var name = User.Claims.FirstOrDefault(c => c.Type == DoctorAccountConfig.Name).Value;
                    var image = new Uri(User.Claims.FirstOrDefault(c => c.Type == DoctorAccountConfig.ImageUri).Value);

                    doctor = doctorController.Read().FirstOrDefault(x => x.Email == email);

                    if (doctor == null)
                    {
                        doctor = new Models.Doctor
                        {
                            Email = email,
                            Name = name,
                            Image = image,
                            Patients = new List<Patient>()
                        };

                        await doctorController.Create(doctor);
                    }
                    else
                    {
                        doctor.Image = image;
                        doctor.Patients = await doctorController.Read(email);
                    }

                    HttpContext.Session.Set(sessionKeyName + email, doctor);
                }

                return View(doctor);
            }
            else
                return Redirect("~/signin");
        }
    }
}