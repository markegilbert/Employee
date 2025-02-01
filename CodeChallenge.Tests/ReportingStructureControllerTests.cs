using CodeChallenge.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NSubstitute;
using CodeChallenge.Controllers;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeCodeChallenge.Tests.Unit
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        // There is test-data bleeding over from the EmployeeControllerTests.  That test suite modifies the in-memory data,
        // so if I were to use the same IDs for testing here, I'd only see the changes.  That would make this suite dependent
        // on that one.
        //
        // If I move ReportingStructureControllerTests to be above EmployeeControllerTests in the list alphabetically, then 
        // ReportingStructureControllerTests runs first against a pristine data set, and all the tests shown here will pass.
        // A couple of ways to re-sort these are through changing the namespace from CodeCodeChallenge.Tests.Integration to
        // something like:
        //      CodeChallenge.Tests.Integration
        //      CodeCodeChallenge.Tests
        // Either will move them above CodeCodeChallenge.Tests.Integration.EmployeeControllerTests in the list.
        //
        // I was not successful in finding a way to reset the in-memory database to the original data.  I tried using the
        // same logic found in App.SeedEmployeeDB(), calling that in the InitializeClass() method here, but that didn't work.
        // I suspect I would have to re-inject that dependency before the controllers will take advantage of them.
        //
        // In the end, I opted to test the ReportingStructureController logic and the new EmployeeService.GetNumberOfReports()
        // method separately, rather than doing an integration test modeled after EmployeeControllerTests.
        #region " Original Test Code "
        //private static HttpClient _httpClient;
        //private static TestServer _testServer;

        //[ClassInitialize]
        //// Attribute ClassInitialize requires this signature
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        //public static void InitializeClass(TestContext context)
        //{
        //    _testServer = new TestServer();
        //    _httpClient = _testServer.NewClient();
        //}

        //[ClassCleanup]
        //public static void CleanUpTest()
        //{
        //    _httpClient.Dispose();
        //    _testServer.Dispose();
        //}


        //[TestMethod]
        //public void GetReportingStructureByEmployeeId_ValidEmployeeRequested_ReturnsOk()
        //{
        //    // Arrange

        //    var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
        //    var expectedFirstName = "John";
        //    var expectedLastName = "Lennon";
        //    var expectedNumberOfReports = 4;

        //    // Execute
        //    var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
        //    var response = getRequestTask.Result;

        //    // Assert
        //    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        //    var reportingStructure = response.DeserializeContent<ReportingStructure>();
        //    Assert.AreEqual(expectedFirstName, reportingStructure.Employee.FirstName, "FirstName didn't match as expected");
        //    Assert.AreEqual(expectedLastName, reportingStructure.Employee.LastName, "LastName didn't match as expected");
        //    Assert.AreEqual(expectedNumberOfReports, reportingStructure.NumberOfReports, "NumberOfReports didn't match as expected");
        //}
        #endregion


        private static ILogger<ReportingStructureController> _logger;
        private static IEmployeeService _employeeService;
        private static ReportingStructureController _controller;
        private static IActionResult _rawResponse;


        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _logger = Substitute.For<ILogger<ReportingStructureController>>();
            _employeeService = Substitute.For<IEmployeeService>();
            _controller = null;
            _rawResponse = null;
        }

        [TestMethod]
        [DataRow("B9B37B3A-B50F-4100-B9AA-56E8C29C7B9C", "Han", 0)]
        [DataRow("36575E65-3FF7-411C-BBC7-DC471C95A958", "Luke", 2)]
        [DataRow("62DC8AE0-39D1-4F26-9333-363B89513116", "Leia", 1)]
        public void GetReportingStructureByEmployeeId_ValidEmployeeRequested_ReturnsOk(String TestEmployeeID, String TestFirstName, int TestNumberOfReports)
        {
            Object rawValue;
            ReportingStructure actualValue;

            // Arrange
            _employeeService.GetById(TestEmployeeID).Returns(new Employee() { EmployeeId = TestEmployeeID, FirstName = TestFirstName });
            _employeeService.GetNumberOfReports(TestEmployeeID).Returns(TestNumberOfReports);
            _controller = new ReportingStructureController(_logger, _employeeService);


            // Execute
            _rawResponse = _controller.GetReportingStructureByEmployeeId(TestEmployeeID);


            // Assert
            Assert.IsTrue(_rawResponse.GetType().Equals(typeof(OkObjectResult)), "The response should have been an OK response");

            rawValue = ((OkObjectResult)_rawResponse).Value;
            Assert.IsNotNull(rawValue, "The response's value should not have been null");

            actualValue = (ReportingStructure)rawValue;
            Assert.AreEqual(TestFirstName, actualValue.Employee.FirstName, "FirstName didn't match as expected");
            Assert.AreEqual(TestNumberOfReports, actualValue.NumberOfReports, "NumberOfReports didn't match as expected");
        }
        [TestMethod]
        public void GetReportingStructureByEmployeeId_InvalidEmployeeRequested_ReturnsNotFound()
        {
            // Arrange
            _controller = new ReportingStructureController(_logger, _employeeService);

            // Execute
            _rawResponse = _controller.GetReportingStructureByEmployeeId("BAD-ID");

            // Assert
            Assert.IsTrue(_rawResponse.GetType().Equals(typeof(NotFoundResult)), "The response should have been a Not Found response");
        }

    }
}
