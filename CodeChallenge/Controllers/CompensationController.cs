using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Controllers
{
    [Route("api/compensation")]
    [ApiController]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;


        public CompensationController(ILogger<CompensationController> logger, IEmployeeService employeeService)
        {
            // TODO: Validate these
            //if (logger == null) { throw new ArgumentNullException($"The parameter '{nameof(logger)}' was null or otherwise invalid"); }
            //if (employeeService == null) { throw new ArgumentNullException($"The parameter '{nameof(employeeService)}' was null or otherwise invalid"); }

            _logger = logger;
            _employeeService = employeeService;
        }



        [HttpGet("{id}", Name = "getCompensationByEmployeeById")]
        public IActionResult GetCompensationByEmployeeId(String id)
        {
            Compensation compensation;

            _logger.LogDebug($"Received compensation get request for employee '{id}'");

            // TODO: Flesh out the real logic here


            return NotFound();
        }
    }
}
