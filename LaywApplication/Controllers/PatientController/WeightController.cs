using LaywApplication.Configuration;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers.PatientController
{
    [Route("~/dashboard/patients/{id}/[controller]")]
    public class WeightController : BaseJsonReadController<PatientWeight>
    {
        public WeightController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Weight) { }
    }
}