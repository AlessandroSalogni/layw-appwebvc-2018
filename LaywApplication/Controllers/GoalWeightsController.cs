using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.APIUtils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

//namespace LaywApplication.Controllers
//{
//    public class GoalWeightsController : ChartDashboardController<GoalsWeight, Weights>
//    {
//        public GoalWeightsController(IOptions<ServerIP> config) : base(config, "goals-weight", "weights") {}

//        //[HttpGet("~/dashboard/[controller]")]
//        //public override ActionResult Read()
//        //{
//        //    List<AchievedGoals> agList = new List<AchievedGoals>();

//        //    foreach (Patient patient in DashboardController.doctor.Patients)
//        //        agList.Add(GetAchievedGoals(patient, DateTime.Now.ToShortDateString().Replace('/', '-')));

//        //    int notAchieved = agList.Count(x => x.Goals.Goal <= x.Summary.Weight);
//        //    int ac = agList.Count - notAchieved;

//        //    var query = from x in agList where x.Goals.Goal <= x.Summary.Weight select x.Name;
//        //    var query2 = from x in agList where x.Goals.Goal > x.Summary.Weight select x.Name;

//        //    if (Request.Query["achieved"].Equals("yes"))
//        //        return Json(query2.ToList());
//        //    else if (Request.Query["achieved"].Equals("no"))
//        //        return Json(query.ToList());
//        //    else
//        //        return Json(new List<object> { new { category = "Achieved", amount = ac, color = "#00E100" }, new { category = "Not Achieved", amount = notAchieved, color = "#FF0000" } });
//        //}

//        public override ActionResult Read(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public override ActionResult ReadAchieved()
//        {
//            throw new NotImplementedException();
//        }

//        public override ActionResult ReadNotAchieved()
//        {
//            throw new NotImplementedException();
//        }

//        public override ActionResult ReadSummary()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}