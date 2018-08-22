﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Configuration
{
    public class JsonStructure
    {
        public Parameters Parameters { get; set; }
        public AdditionalPath AdditionalPath { get; set; }
        public GoalsStepsDaily GoalsStepsDaily { get; set; }
        public GoalsWeight GoalsWeight { get; set; }
        public GoalCaloriesOut GoalsCaloriesOut { get; set; }
        public StepsSummary StepsSummary{ get; set; }
        public WeightSummary WeightSummary { get; set; }
        public CaloriesSummary CaloriesSummary { get; set; }
    }

    public class Parameters
    {
        public string Period { get; set; }
        public string Date { get; set; }
    }
    public class AdditionalPath
    {
        public string Current { get; set; }
    }
    public class JsonData
    {
        public string Url { get; set; }
        public string Root { get; set; }
        public string Key { get; set; }
    }
    public class GoalsStepsDaily : JsonData { }
    public class GoalsWeight : JsonData { }
    public class GoalCaloriesOut : JsonData { }
    public class StepsSummary : JsonData { }
    public class CaloriesSummary : JsonData
    {
        public string Object { get; set; }
    }
    public class WeightSummary : JsonData { }
}