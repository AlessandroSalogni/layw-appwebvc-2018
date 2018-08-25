﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaywApplication.Configuration;
using LaywApplication.Controllers.Utils;
using LaywApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaywApplication.Controllers
{
    public class TrainingController : Controller
    {
        private static readonly object Empty = new { };
        private readonly IOptions<ServerIP> config;

        public TrainingController(IOptions<ServerIP> config)
        {
            this.config = config;
        }

        [HttpGet("~/dashboard/patients/{id}/[controller]")]
        public async Task<IEnumerable<Training>> Read(int id)
        {   
            JObject jsonTraining = await APIUtils.GetAsync(config.Value.GetTotalUrlUser() + id + "/trainings"); //todo mettere path nel config
            return ((JArray)jsonTraining.GetValue("training-days")).GetList<Training>();
        }

        [HttpPost("~/dashboard/training/create")]
        public object Create([FromBody]Training item)
        {
            return Empty;
        }

        [HttpPost("~/dashboard/training/update")]
        public object Update([FromBody]Training item)
        {
            return Empty;
        }

        [HttpPost("~/dashboard/training/delete")]
        public void Delete([FromBody]Training item)
        {
        }
    }
}