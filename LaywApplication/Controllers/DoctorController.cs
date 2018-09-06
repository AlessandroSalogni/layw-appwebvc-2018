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
        public List<Models.Doctor> Read()
        {
            var doctorConfig = JsonDataConfig as Configuration.Doctor;

            JObject doctorsJson = APIUtils.Get(IPConfig.GetTotalUrl() + doctorConfig.Url);
            return (doctorsJson == null) ? new List<Models.Doctor>() :
                ((JArray)doctorsJson[doctorConfig.RootDoctors]).GetList<Models.Doctor>();
        }

        [HttpGet("{email}")]
        public async Task<List<Patient>> Read(string email)
        {
            var doctorConfig = JsonDataConfig as Configuration.Doctor;

            JObject patientsJson = await APIUtils.GetAsync(IPConfig.GetTotalUrl() + doctorConfig.UrlPatients + 
                "?" + QueryParamsConfig.DoctorId + "=" + email);
            return (patientsJson == null) ? new List<Patient>() :
                ((JArray)patientsJson[doctorConfig.RootPatients]).GetList<Patient>();
        }

        [HttpPut("{email}/update")]
        public object Update(string email, [FromBody]List<Patient> item)
        {
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