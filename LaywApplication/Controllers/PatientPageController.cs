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
    public class PatientPageController : BaseController
    {
        private readonly DoctorAccount DoctorAccountConfig;
        private readonly JsonStructure JsonStructureConfig;

        public PatientPageController(ServerIP IPConfig, JsonStructure jsonStructureConfig,
            DoctorAccount doctorAccountConfig) : base(IPConfig)
        {
            DoctorAccountConfig = doctorAccountConfig;
            JsonStructureConfig = jsonStructureConfig;
        }

        [HttpGet("~/dashboard/patients/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var doctorEmail = User.Claims.FirstOrDefault(c => c.Type == DoctorAccountConfig.Email).Value;
                var doctor = HttpContext.Session.Get<Models.Doctor>(sessionKeyName + doctorEmail);

                if (doctor == null)
                {
                    doctor = new Models.Doctor
                    {
                        Email = doctorEmail,
                        Name = User.Claims.FirstOrDefault(c => c.Type == DoctorAccountConfig.Name).Value,
                        Image = new Uri(User.Claims.FirstOrDefault(c => c.Type == DoctorAccountConfig.ImageUri).Value),
                        Patients = await (new DoctorController(IPConfig, JsonStructureConfig).Read(doctorEmail))
                    };
                    HttpContext.Session.Set(sessionKeyName + doctorEmail, doctor);
                }

                var patient = doctor.Patients.FirstOrDefault(x => x.Id == id);
                if (patient == null)
                    return Redirect("~/dashboard/homepage");

                ViewBag.CurrentPatient = patient;
                return View(doctor);
            }
            else
                return Redirect("~/signin");
        }
    }
}