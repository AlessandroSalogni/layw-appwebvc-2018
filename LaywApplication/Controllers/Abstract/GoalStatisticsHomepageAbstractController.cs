using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.PatientController;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers
{
    public abstract class GoalStatisticsHomepageAbstractController<TType, TModelGoal, TModelRealData> : BaseJsonController
        where TType : struct, IComparable<TType>
        where TModelGoal : GoalAbstract<TType>
        where TModelRealData : RealDataAbstract<TType>
    {
        protected readonly BaseJsonReadController<TModelGoal> GoalController;
        protected readonly BaseJsonReadController<TModelRealData> RealDataController;

        private struct GoalRealDataCompare
        {
            public TType Goal;
            public TType RealData;
            public string PatientName;
        }

        public GoalStatisticsHomepageAbstractController(ServerIP IPConfig, JsonStructure jsonStructureConfig,
            BaseJsonReadController<TModelGoal> goalController,
            BaseJsonReadController<TModelRealData> realDataController)
            : base(IPConfig, jsonStructureConfig)
        {
            GoalController = goalController;
            RealDataController = realDataController;
        }

        [HttpGet("achieved")]
        public async Task<List<Models.Patient>> ReadAchieved(string doctorEmail)
            => await GetListPatient(doctorEmail, GoalIsAchieved);

        [HttpGet("notachieved")]
        public async Task<List<Models.Patient>> ReadNotAchieved(string doctorEmail)
            => await GetListPatient(doctorEmail, (x,y) => !GoalIsAchieved(x,y));

        private async Task<List<Models.Patient>> GetListPatient(string doctorEmail, Func<TType, TType, bool> condition)
        {
            var goalRealDataCompareList = await GetGoalRealDataCompareListAsync(doctorEmail);

            List<Models.Patient> patientList = new List<Models.Patient>();
            foreach (string name in from x in goalRealDataCompareList where condition(x.Goal, x.RealData) select x.PatientName)
                patientList.Add(new Models.Patient { Name = name });

            return patientList;
        }

        [HttpGet("summary")]
        public async Task<ActionResult> ReadSummary(string doctorEmail)
        {
            var goalRealDataCompareList = await GetGoalRealDataCompareListAsync(doctorEmail);

            int notAchieved = goalRealDataCompareList.Count(x => !GoalIsAchieved(x.Goal, x.RealData));
            int achieved = goalRealDataCompareList.Count - notAchieved;

            return Json(new List<object> { new { category = "Achieved", amount = achieved, color = "#00E100" }, new { category = "Not Achieved", amount = notAchieved, color = "#FF0000" } });
        }

        abstract protected bool GoalIsAchieved(TType goal, TType realData);

        private async Task<List<GoalRealDataCompare>> GetGoalRealDataCompareListAsync(string doctorEmail)
        {
            List<GoalRealDataCompare> goalRealDataCompareList = new List<GoalRealDataCompare>();

            var dateNow = DateTimeNow.ToString(italianDateFormat);
            var doctor = HttpContext.Session.Get<Models.Doctor>(sessionKeyName + doctorEmail);

            foreach (Models.Patient patient in doctor.Patients)
            {
                var realData = await RealDataController.Read(patient.Id, dateNow);
                var realDataValue = (realData != null) ? realData.RealData : default(TType);

                goalRealDataCompareList.Add(new GoalRealDataCompare
                {
                    Goal = (await GoalController.Read(patient.Id, dateNow)).Goal,
                    RealData = realDataValue,
                    PatientName = patient.Name
                });
            }
            
            return goalRealDataCompareList;
        }
    }
}
