using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHome()
        {
            return Ok("Service Ready!, Access service thru /api");
        }
        [Route("api")]
        [HttpGet]
        public IActionResult GetApi()
        {
            return Ok("Service requires Authentication, please login before proceeding!");
        }
    }
}
