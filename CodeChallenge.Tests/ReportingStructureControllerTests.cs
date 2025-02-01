using CodeChallenge.Controllers;
using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace CodeCodeChallenge.Tests.Unit
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        // There is test-data leaking over from the EmployeeControllerTests.  That test suite modifies the in-memory data,
        // so if I were to use the same IDs for testing here with no changes to the code, those changes would interfere with
        // the tests here.  I.e., this suite would be dependent on that one.
        //
        // Initially, I didn't find a way to reset the in-memory database to avoid the data leakage, so I separated the
        // integration tests for ReportingStructureController into 1) unit tests of the controller using mock objects,
        // and 2) unit tests of the new EmployeeService.GetNumberOfReports() method.
        //
        // I did eventually find a way to reset the database between test suites.  That reset logic can be found in the
        // EmployeeServiceTests.InitializeClass() and CleanupClass() methods.
        // 
        // I decided to keep the ReportingStructureControllerTests structure as it was since it was a valid test of
        // the controller logic.  Combined with the EmployeeServiceTests suite, we still have full test coverage of the
        // new endpoint all the way down to the database level.


        private static ILogger<ReportingStructureController> _logger;
        private static IEmployeeService _employeeService;
        private ReportingStructureController _controller;
        private IActionResult _rawResponse;


        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _logger = Substitute.For<ILogger<ReportingStructureController>>();
            _employeeService = Substitute.For<IEmployeeService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
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
