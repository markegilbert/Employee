using CodeChallenge.Controllers;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;


namespace CodeCodeChallenge.Tests.Unit
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static ILogger<CompensationController> _logger;
        private static IEmployeeService _employeeService;
        private static ICompensationService _compensationService;
        private CompensationController _controller;
        private IActionResult _rawResponse;
        private ArgumentNullException _argumentNullException;


        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _logger = Substitute.For<ILogger<CompensationController>>();
            _employeeService = Substitute.For<IEmployeeService>();
            _compensationService = Substitute.For<ICompensationService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _controller = null;
            _rawResponse = null;
            _argumentNullException = null;
        }



        [TestMethod]
        public void InstantiateClass_LoggerIsNull_ExceptionThrown()
        {
            _argumentNullException = Assert.ThrowsException<ArgumentNullException>(() => new CompensationController(null, _employeeService, _compensationService));
            Assert.IsTrue(_argumentNullException.Message.Contains("'logger'"), "Exception didn't reference the correct parameter");
        }
        [TestMethod]
        public void InstantiateClass_EmployeeServiceIsNull_ExceptionThrown()
        {
            _argumentNullException = Assert.ThrowsException<ArgumentNullException>(() => new CompensationController(_logger, null, _compensationService));
            Assert.IsTrue(_argumentNullException.Message.Contains("'employeeService'"), "Exception didn't reference the correct parameter");
        }
        [TestMethod]
        public void InstantiateClass_CompensationServiceIsNull_ExceptionThrown()
        {
            _argumentNullException = Assert.ThrowsException<ArgumentNullException>(() => new CompensationController(_logger, _employeeService, null));
            Assert.IsTrue(_argumentNullException.Message.Contains("'compensationService'"), "Exception didn't reference the correct parameter");
        }


        // TODO: Add unit tests for GetCompensationByEmployeeId(), mocking out the services like I did with ReportingStructureControllerTests
        // TODO: Add unit tests for CreateCompensation(), mocking out the services like I did with ReportingStructureControllerTests
    }
}
