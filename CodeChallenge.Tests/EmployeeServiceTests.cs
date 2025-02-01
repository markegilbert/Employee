using CodeChallenge.Data;
using CodeChallenge.Repositories;
using CodeChallenge.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;


namespace CodeCodeChallenge.Tests.Unit
{
    [TestClass]
    public class EmployeeServiceTests
    {

        private static ILogger<EmployeeService> _employeeServiceLogger;
        private static ILogger<IEmployeeRepository> _employeeRepositoryLogger;
        private static IEmployeeService _employeeService;
        private static int _actualNumberOfReports;
        private static EmployeeContext _employeeContext;


        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            // Drop any previous in-memory database to ensure that there is no data-leakage from another test suite.
            _employeeContext = new EmployeeContext(new DbContextOptionsBuilder<EmployeeContext>().UseInMemoryDatabase("EmployeeDB").Options);
            _employeeContext.Database.EnsureDeleted();

            // Then re-seed the database to get a pristine copy
            new EmployeeDataSeeder(_employeeContext).Seed().Wait();

            // Initialize an EmployeeService object
            _employeeServiceLogger = Substitute.For<ILogger<EmployeeService>>();
            _employeeRepositoryLogger = Substitute.For<ILogger<IEmployeeRepository>>();
            _employeeService = new EmployeeService(_employeeServiceLogger, 
                                new EmployeeRespository(_employeeRepositoryLogger, _employeeContext));
            
        }

        [ClassCleanup]
        public static void CleanupClass()
        {
            if (_employeeContext != null) { _employeeContext.Database.EnsureDeleted(); }
        }


        [TestMethod]
        [DataRow("16a596ae-edd3-4847-99fe-c4518e82c86f", 4)]
        [DataRow("b7839309-3348-463b-a7e3-5de1c168beb3", 0)]
        [DataRow("03aa1462-ffa9-4978-901b-7c001562cf6f", 2)]
        [DataRow("62c1084e-6e34-4630-93fd-9153afb65309", 0)]
        [DataRow("c0c2293d-16bd-4603-8e08-638a9d18b22c", 0)]
        public void GetNumberOfReports_ValidIdRequested_CorrectNumberOfReportsReturned(string employeeId, int expectedNumberOfReports)
        {
            // Arrange

            // Execute
            _actualNumberOfReports = _employeeService.GetNumberOfReports(employeeId);

            // Assert
            Assert.AreEqual(expectedNumberOfReports, _actualNumberOfReports, $"Number of reports for Employee ID {employeeId} was not correct");
        }
        [TestMethod]
        public void GetNumberOfReports_InvalidIdRequested_NumberOfReportsReturnedIs0()
        {
            // Arrange
            string employeeId = "BAD-ID";

            // Execute
            _actualNumberOfReports = _employeeService.GetNumberOfReports(employeeId);

            // Assert
            Assert.AreEqual(0, _actualNumberOfReports, $"Number of reports for Employee ID {employeeId} was not correct");
        }
    }
}
