﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Services.PatientData;
using LaywApplication.Controllers.Services.PatientData.Abstract;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace LaywApplication.Controllers.Abstract
{
    public abstract class GoalStatisticsHomepageAbstractController<TType, TModelGoal, TModelRealData> : BaseJsonController
        where TType : struct, IComparable<TType>
        where TModelGoal : GoalAbstract<TType>
        where TModelRealData : RealDataAbstract<TType>
    {
        protected readonly BaseJsonReadController<TModelGoal> GoalController;
        protected readonly BaseJsonReadController<TModelRealData> RealDataController;
        protected readonly GeneralHomepageChartInfo HomepageChartInfo;

        private struct GoalRealDataCompare
        {
            public TType Goal;
            public TType RealData;
            public string PatientName;
        }

        public GoalStatisticsHomepageAbstractController(ServerIP IPConfig, JsonStructure jsonStructureConfig,
            ChartHomepageInfo homepageChartInfo,
            BaseJsonReadController<TModelGoal> goalController,
            BaseJsonReadController<TModelRealData> realDataController)
            : base(IPConfig, jsonStructureConfig)
        {
            GoalController = goalController;
            RealDataController = realDataController;
            HomepageChartInfo = homepageChartInfo.GeneralChartInfo;
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
        public async Task<JsonResult> ReadSummary(string doctorEmail)
        {
            var goalRealDataCompareList = await GetGoalRealDataCompareListAsync(doctorEmail);

            int notAchieved = goalRealDataCompareList.Count(x => !GoalIsAchieved(x.Goal, x.RealData));
            int achieved = goalRealDataCompareList.Count - notAchieved;

            return Json(new List<object> { new { category = HomepageChartInfo.Legend[0], amount = achieved,
                color = HomepageChartInfo.Color[0] }, new { category = HomepageChartInfo.Legend[1],
                amount = notAchieved, color = HomepageChartInfo.Color[1] } });
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
                var goal = await GoalController.Read(patient.Id, dateNow);

                goalRealDataCompareList.Add(new GoalRealDataCompare
                {
                    Goal = (goal != null) ? goal.Goal : default(TType),
                    RealData = (realData != null) ? realData.RealData : default(TType),
                    PatientName = patient.Name
                });
            }
            
            return goalRealDataCompareList;
        }
    }
}
