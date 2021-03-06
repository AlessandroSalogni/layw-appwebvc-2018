﻿using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Services.PatientData;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.ViewComponents
{
    public class PatientActivitiesViewComponent : BaseViewComponent
    {
        private readonly ActivityController ActivityController;

        public PatientActivitiesViewComponent(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig)
        {
            ActivityController = new ActivityController(IPConfig, jsonStructureConfig);
        }

        public async Task<IViewComponentResult> InvokeAsync(Models.Patient currentPatient)
        {
            ViewBag.AerobicFunction = new AerobicFunction(currentPatient);
            return View(await ActivityController.Read(currentPatient.Id, DateTimeNow.ToString(italianDateFormat), "1d"));
        }
    }
}