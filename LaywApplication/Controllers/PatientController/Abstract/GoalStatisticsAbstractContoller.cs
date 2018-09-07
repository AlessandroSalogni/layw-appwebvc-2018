using LaywApplication.Configuration;
using LaywApplication.Models;
using LaywApplication.Models.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaywApplication.Controllers.PatientController.Abstract
{
    public abstract class GoalStatisticsController<TType, TModelGoal, TModelRealData> : BaseJsonController
      where TType : struct, IComparable<TType>
      where TModelGoal : GoalAbstract<TType>
      where TModelRealData : RealDataAbstract<TType>
    {
        protected readonly BaseJsonReadController<TModelGoal> GoalController;
        protected readonly BaseJsonReadController<TModelRealData> RealDataController;

        public GoalStatisticsController(ServerIP IPConfig, JsonStructure jsonStructureConfig,
            BaseJsonReadController<TModelGoal> goalController,
            BaseJsonReadController<TModelRealData> realDataController)
            : base(IPConfig, jsonStructureConfig)
        {
            GoalController = goalController;
            RealDataController = realDataController;
        }

        [HttpGet]
        public async Task<List<GoalRealDataCompare<TType>>> Read(int id, string beginDate, string period)
        {
            var goalReadDataCompareList = new List<GoalRealDataCompare<TType>>();
            List<TModelRealData> realDataList = await RealDataController.Read(id, beginDate, period);

            TModelGoal goal = null;
            TModelRealData realData = null;

            for (int i = realDataList.Count - 1; i > -1; i--)
            {
                realData = realDataList[i];
                if (goal == null || DateTime.Compare(goal.Date, realData.Date) > 0)
                    goal = await GoalController.Read(id, realData.Date.ToString(italianDateFormat));

                goalReadDataCompareList.Insert(0, new GoalRealDataCompare<TType>
                {
                    Goal = goal.Goal,
                    RealData = realData.RealData,
                    Date = realData.Date.ToString(italianDateFormat)
                });
            }

            return goalReadDataCompareList;
        }
    }

}
