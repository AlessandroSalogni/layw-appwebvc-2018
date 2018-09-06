using System.Collections.Generic;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    [Route("~/dashboard/doctors")]
    public class DoctorController : BaseJsonController
    {
        public DoctorController(ServerIP IPConfig, JsonStructure jsonStructure) 
            : base(IPConfig, jsonStructure, jsonStructure.Doctor) { }

        [HttpGet]
        public async Task<List<Models.Doctor>> Read()
        {
            var doctorConfig = JsonDataConfig as Configuration.Doctor;

            JObject doctorsJson = await APIUtils.GetAsync(IPConfig.GetTotalUrl() + doctorConfig.Url);

            if (doctorsJson == null)
                return new List<Models.Doctor>();
            else
            {
                var doctors = ((JArray)doctorsJson[doctorConfig.RootDoctors]).GetList<Models.Doctor>();
                foreach (Models.Doctor doctor in doctors)
                {
                    JObject patientsJson = await APIUtils.GetAsync(IPConfig.GetTotalUrl() + doctorConfig.UrlPatients + "?" + QueryParamsConfig.DoctorId + "=" + doctor.Email);
                    if (patientsJson == null)
                        doctor.Patients = new List<Models.Patient>();
                    else
                        doctor.Patients = ((JArray)patientsJson[doctorConfig.RootPatients]).GetList<Models.Patient>();
                }
                return doctors;
            }
        }

        [HttpGet("{email}")]
        public async Task<List<Models.Patient>> Read(string email)
        {
            var doctorConfig = JsonDataConfig as Configuration.Doctor;

            JObject patientsJson = await APIUtils.GetAsync(IPConfig.GetTotalUrl() + doctorConfig.UrlPatients + 
                "?" + QueryParamsConfig.DoctorId + "=" + email);
            return (patientsJson == null) ? new List<Models.Patient>() :
                ((JArray)patientsJson[doctorConfig.RootPatients]).GetList<Models.Patient>();
        }

        [HttpPut("{email}/update")]
        public async Task<object> Update(string email, [FromBody]List<Models.Patient> item)
        {
            var doctorConfig = JsonDataConfig as Configuration.Doctor;
            
            var patientListIdJson = new JArray();
            item.ForEach(x => patientListIdJson.Add(x.Id));

            var patientListJson = new JObject
            {
                { doctorConfig.RootPatients, patientListIdJson }
            };

            await APIUtils.PostAsync(IPConfig.GetTotalUrl() + JsonDataConfig.Url + "/" + email , patientListJson.ToString());
            return Empty;
        }

        [HttpPost("create")]
        public async Task<object> Create([FromBody]Models.Doctor item)
        {
            var doctorJson = new JObject
            {
                { JsonDataConfig.Root , JObject.Parse(JsonConvert.SerializeObject(item, serializerSettings)) }
            };

            await APIUtils.PostAsync(IPConfig.GetTotalUrl() + JsonDataConfig.Url, doctorJson.ToString());
            return Empty;
        }
    }
}