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
        private readonly ICompensationService _compensationService;


        public CompensationController(ILogger<CompensationController> logger, IEmployeeService employeeService, ICompensationService compensationService)
        {
            // TODO: Validate these
            //if (logger == null) { throw new ArgumentNullException($"The parameter '{nameof(logger)}' was null or otherwise invalid"); }
            //if (employeeService == null) { throw new ArgumentNullException($"The parameter '{nameof(employeeService)}' was null or otherwise invalid"); }

            _logger = logger;
            _employeeService = employeeService;
            _compensationService = compensationService;
        }



        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeById")]
        public IActionResult GetCompensationByEmployeeId(String employeeId)
        {
            Compensation compensation;

            _logger.LogDebug($"Received compensation get request for employee '{employeeId}'");

            compensation = _compensationService.GetByEmployeeId(employeeId);
            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }


        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            Employee employee;

            _logger.LogDebug($"Received compensation create request for employee ID '{compensation.Employee.EmployeeId}'");

            employee = _employeeService.GetById(compensation.Employee.EmployeeId);
            if (employee == null)
                return NotFound();


            // TODO: Is this an acceptable way to force compensation.Employee to be populated?
            // TODO: Does it matter that when this record is retrieved, compensation.Employee.DirectReports is null and compensation.Employee.Compensations is a list that only contains "null"?
            compensation.Employee = employee;
            compensation = _compensationService.Create(compensation);

            return CreatedAtRoute("getCompensationByEmployeeById", new { compensation.Employee.EmployeeId }, compensation);
        }

    }
}
