using LaywApplication.Configuration;
using LaywApplication.Controllers.Services.PatientData.Abstract;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers.Services.PatientData
{
    [Route("~/dashboard/patients/{id}/[controller]")]
    public class WeightController : BaseJsonReadController<PatientWeight>
    {
        public WeightController(ServerIP IPConfig, JsonStructure jsonStructureConfig) 
            : base(IPConfig, jsonStructureConfig, jsonStructureConfig.Weight) { }
    }
}