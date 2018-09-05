using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Configuration
{
    public class JsonStructure
    {
        public QueryParams QueryParams { get; set; }
        public AdditionalPath AdditionalPath { get; set; }
        public Doctor Doctor { get; set; }
        public GoalsStepsDaily GoalsStepsDaily { get; set; }
        public GoalsWeight GoalsWeight { get; set; }
        public GoalCaloriesOut GoalsCaloriesOut { get; set; }
        public Activities Activities { get; set; }
        public ActivitySummary ActivitySummary { get; set; }
        public Weight Weight { get; set; }
        public Diet Diet { get; set; }
        public Training Training { get; set; }
    }

    public class QueryParams
    {
        public string Period { get; set; }
        public string Date { get; set; }
        public string DoctorId { get; set; }
    }
    public class AdditionalPath
    {
        public string Current { get; set; }
    }
    public class JsonData
    {
        public string Url { get; set; }
        public string Root { get; set; }
        public string[] Key { get; set; }
    }
    public class Doctor : JsonData
    {
        public string UrlPatients { get; set; }
        public string RootPatients { get; set; }
    }
    public class GoalsStepsDaily : JsonData
    {
        public string UrlUpdate { get; set; }
        public string RootUpdate { get; set; }
        public string Period { get; set; }
    }
    public class GoalsWeight : JsonData { }
    public class GoalCaloriesOut : JsonData { }
    public class Activities : JsonData { }
    public class ActivitySummary : JsonData { }
    public class Weight : JsonData { }
    public class Diet : JsonData { }
    public class Training : JsonData { }
}
