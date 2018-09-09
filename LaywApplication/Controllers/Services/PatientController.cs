using System.Collections.Generic;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Abstract;
using LaywApplication.Controllers.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers.Services
{
    [Route("~/dashboard/patients")]
    public class PatientController : BaseJsonController
    {
        public PatientController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Patient) { }

        [HttpGet]
        public async Task<List<Models.Patient>> Read()
        {
            var patientConfig = JsonDataConfig as Patient;

            JObject patientsJson = await APIUtils.GetAsync(IPConfig.GetTotalUrl() + patientConfig.Url);
            return (patientsJson == null) ? new List<Models.Patient>() :
                ((JArray)patientsJson[patientConfig.RootPatients]).GetList<Models.Patient>();
        }

        [HttpGet("{id}/data")]
        public async Task<Models.Patient> Read(int id)
        {
            JObject patientsJson = await APIUtils.GetAsync(IPConfig.GetTotalUrl() + JsonDataConfig.Url + "/" + id);
            return (patientsJson == null) ? null :
                ((JObject)patientsJson[JsonDataConfig.Root]).GetObject<Models.Patient>();
        }
    }
}