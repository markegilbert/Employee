using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Controllers
{
    [Route("api/reportingstructure")]
    [ApiController]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;


        public ReportingStructureController(ILogger<ReportingStructureController> logger, IEmployeeService employeeService)
        {
            // TODO: Validate these
            _logger = logger;
            _employeeService = employeeService;
        }


        // TODO: Unit tests for this endpoint
        // TODO: Mock out GetById and GetNumberOfReports methods
        [HttpGet("{id}", Name = "getReportingStructureByEmployeeById")]
        public IActionResult GetReportingStructureByEmployeeId(String id)
        {
            ReportingStructure reportingStructure;
            int numberOfReports;

            _logger.LogDebug($"Received reporting structure get request for employee '{id}'");

            // Get the employee requested
            var employee = _employeeService.GetById(id);
            if (employee == null)
                return NotFound();

            // Retrieve the number of reports for this employee
            numberOfReports = _employeeService.GetNumberOfReports(id);


            // I found that calling _employeeService.GetNumberOfReports will also update the local employee's DirectReport property, and flesh
            // out the entire tree structure, down to the leaves.  Previously, the employee.DirectReports property was null.  Now it's either a 
            // list of Employee objects or an empty list.  If this side effect is problematic, it can be resolved as follows:
            //
            //      employee.DirectReports = null;
            //
            // If that is the case, this resolution should definitely get unit test coverage.


            // Assemble the response object
            reportingStructure = new ReportingStructure() { Employee = employee, NumberOfReports = numberOfReports };

            return Ok(reportingStructure);
        }
    }
}
