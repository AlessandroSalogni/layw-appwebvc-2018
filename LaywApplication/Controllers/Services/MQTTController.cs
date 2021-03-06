﻿using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Abstract;
using LaywApplication.Controllers.Services.PatientData;
using LaywApplication.Models.PatientData;
using LaywApplication.Mqtt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LaywApplication.Controllers.Services
{
    [Route("~/dashboard/mqtt")]
    public class MQTTController : BaseController
    {
        private readonly IHubContext<MqttHub> HubContext;
        private readonly JsonStructure JsonStructureConfig;
        private readonly JsonData WeightConfig;
        private readonly JsonData ActivitySummaryConfig;

        public MQTTController(ServerIP IPConfig, JsonStructure jsonStructureConfig, IHubContext<MqttHub> hubContext)
            : base(IPConfig)
        {
            JsonStructureConfig = jsonStructureConfig;
            WeightConfig = jsonStructureConfig.Weight;
            ActivitySummaryConfig = jsonStructureConfig.ActivitySummary;
            HubContext = hubContext;
        }

        [HttpPost("weight")]
        public async Task<object> SendWeightNotification(int patientId, string doctorEmail, [FromBody]PatientWeight item)
        {
            Models.Patient patient = await new PatientController(IPConfig, JsonStructureConfig).Read(patientId);
            var goalWeight = await new GoalWeightController(IPConfig, JsonStructureConfig)
                .Read(patientId, DateTimeNow.ToString(italianDateFormat));

            if (item.Weight <= goalWeight.Goal)
                await HubContext.Clients.All.SendAsync(doctorEmail + "/" + WeightConfig.Key[0], patient.Name, patientId);

            return Empty;
        }

        [HttpPost("activitysummary")]
        public async Task<object> SendActivitySummaryNotification(int patientId, string doctorEmail, [FromBody]Models.PatientData.ActivitySummary item)
        {
            Models.Patient patient = await new PatientController(IPConfig, JsonStructureConfig).Read(patientId);
            var goalSteps = await new GoalStepsDailyController(IPConfig, JsonStructureConfig)
                .Read(patientId, DateTimeNow.ToString(italianDateFormat));
            var goalCalories = await new GoalCaloriesOutController(IPConfig, JsonStructureConfig)
                .Read(patientId, DateTimeNow.ToString(italianDateFormat));

            if (item.Steps >= goalSteps.Goal)
                await HubContext.Clients.All.SendAsync(doctorEmail + "/" + ActivitySummaryConfig.Key[0], patient.Name, patientId);
            if (item.CaloriesCategory.OutCalories >= goalCalories.Goal)
                await HubContext.Clients.All.SendAsync(doctorEmail + "/" + ActivitySummaryConfig.Key[1], patient.Name, patientId);

            return Empty;
        }
    }
}