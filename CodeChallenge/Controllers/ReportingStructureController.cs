using CodeChallenge.Models;
using CodeChallenge.Services;
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
            if (logger == null) { throw new ArgumentNullException($"The parameter '{nameof(logger)}' was null or otherwise invalid"); }
            if (employeeService == null) { throw new ArgumentNullException($"The parameter '{nameof(employeeService)}' was null or otherwise invalid"); }

            _logger = logger;
            _employeeService = employeeService;
        }


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


            // TODO: I found that calling _employeeService.GetNumberOfReports will also update the local employee's DirectReport property, and flesh
            // out the entire tree structure down to the leaves.  Previously, the employee.DirectReports property was null; now it's either a 
            // list of Employee objects or an empty list.
            //
            // If this side effect is problematic, it can be resolved as follows:
            //
            //      employee.DirectReports = null;
            //
            // If that is the case, a unit test should be added to cover that modification.


            // Assemble the response object
            reportingStructure = new ReportingStructure() { Employee = employee, NumberOfReports = numberOfReports };

            return Ok(reportingStructure);
        }
    }
}
