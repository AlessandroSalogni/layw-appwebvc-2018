using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.PatientController;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    [Route("~/dashboard/patients")]
    public class PatientCollectionController : BaseJsonController
    {
        public PatientCollectionController(ServerIP IPConfig, JsonStructure jsonStructureConfig) : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Patient) {}

        [HttpGet]
        public async Task<List<Models.Patient>> Read()
        {
            var patientConfig = JsonDataConfig as Configuration.Patient;

            JObject jsonPatients = await APIUtils.GetAsync(IPConfig.GetTotalUrl() + patientConfig.Url);
            List<Models.Patient> patients = ((JArray)jsonPatients[patientConfig.RootPatients]).GetList<Models.Patient>();
            
            return patients;
        }
    }
}