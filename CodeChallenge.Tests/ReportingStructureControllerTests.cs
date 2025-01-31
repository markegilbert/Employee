using CodeChallenge.Data;
using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using CodeChallenge.Controllers;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        // There is test-data bleeding over from the EmployeeControllerTests.  That test suite modifies the in-memory data,
        // so if I were to use the same IDs for testing here, this suite would be dependent on it.
        //
        // If I move ReportingStructureControllerTests to be above EmployeeControllerTests in the list, then 
        // ReportingStructureControllerTests runs first against a pristine data set, and they will all pass.  A couple of
        // ways to do this are through changing the namespace from CodeCodeChallenge.Tests.Integration to something like:
        //      CodeChallenge.Tests.Integration
        //      CodeCodeChallenge.Tests
        // Either will move them above CodeCodeChallenge.Tests.Integration.EmployeeControllerTests in the list.
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


        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _logger = Substitute.For<ILogger<ReportingStructureController>>();
            _employeeService = Substitute.For<IEmployeeService>();
        }

        [TestMethod]
        [DataRow("16a596ae-edd3-4847-99fe-c4518e82c86f", "John", 4)]
        [DataRow("b7839309-3348-463b-a7e3-5de1c168beb3", "Paul", 0)]
        [DataRow("03aa1462-ffa9-4978-901b-7c001562cf6f", "Ringo", 2)]
        public void GetReportingStructureByEmployeeId_ValidEmployeeRequested_ReturnsOk(String TestEmployeeID, String TestFirstName, int TestNumberOfReports)
        {
            // Arrange
            _employeeService.GetById(TestEmployeeID).Returns(new Employee() { EmployeeId = TestEmployeeID, FirstName = TestFirstName });
            _employeeService.GetNumberOfReports(TestEmployeeID).Returns(TestNumberOfReports);
            _controller = new ReportingStructureController(_logger, _employeeService);


            // Execute
            // TODO: Shouldn't assume that this comes back as OK
            var response = (OkObjectResult) _controller.GetReportingStructureByEmployeeId(TestEmployeeID);


            // Assert
            Assert.AreEqual(200, response.StatusCode);
            var reportingStructure = (ReportingStructure)response.Value;
            Assert.AreEqual(TestFirstName, reportingStructure.Employee.FirstName, "FirstName didn't match as expected");
            Assert.AreEqual(TestNumberOfReports, reportingStructure.NumberOfReports, "NumberOfReports didn't match as expected");
        }


        // TODO: Add a test for an invalid ID
    }
}
