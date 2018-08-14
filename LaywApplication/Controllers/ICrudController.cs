using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Controllers
{
    interface ICrudController
    {
        [HttpGet]
        ActionResult Read();

        [HttpGet]
        ActionResult Read(int id);
    }
}
