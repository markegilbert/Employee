using CodeChallenge.Data;
using CodeChallenge.Models;
using CodeChallenge.Repositories;
using CodeChallenge.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;


namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationServiceTests
    {
        private static ILogger<CompensationService> _compensationServiceLogger;
        private static ILogger<ICompensationRepository> _compensationRepositoryLogger;
        private static ICompensationService _compensationService;
        private static EmployeeContext _employeeContext;
        private ArgumentNullException _argumentNullException;
        private Compensation _testCompensation, _actualCompensation;
        private String _employeeId;



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

            // Initialize the CompensationService object
            _compensationServiceLogger = Substitute.For<ILogger<CompensationService>>();
            _compensationRepositoryLogger = Substitute.For<ILogger<ICompensationRepository>>();
            _compensationService = new CompensationService(_compensationServiceLogger,
                                new CompensationRepository(_compensationRepositoryLogger, _employeeContext));

        }

        [TestInitialize]
        public void TestInitialize()
        {
            _argumentNullException = null;
            _testCompensation = null;
            _actualCompensation = null;
            _employeeId = null;
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clear out any Compensation records added in the previous test so they don't leak into the next one.
            // Source: https://stackoverflow.com/questions/10448684/how-should-i-remove-all-elements-in-a-dbset
            _employeeContext.Compensations.RemoveRange(_employeeContext.Compensations);
        }



        [TestMethod]
        public void InstantiateClass_LoggerIsNull_ExceptionThrown()
        {
            _argumentNullException = Assert.ThrowsException<ArgumentNullException>(() => new CompensationService(null, new CompensationRepository(_compensationRepositoryLogger, _employeeContext)));
            Assert.IsTrue(_argumentNullException.Message.Contains("'logger'"), "Exception didn't reference the correct parameter");
        }
        [TestMethod]
        public void InstantiateClass_CompensationRepositoryIsNull_ExceptionThrown()
        {
            _argumentNullException = Assert.ThrowsException<ArgumentNullException>(() => new CompensationService(_compensationServiceLogger, null));
            Assert.IsTrue(_argumentNullException.Message.Contains("'compensationRepository'"), "Exception didn't reference the correct parameter");
        }


        [TestMethod]
        public void Create_CompensationParameterIsNull_RecordIsNotSaved()
        {
            // Arrange
            _testCompensation = null;

            // Execute
            _actualCompensation = _compensationService.Create(_testCompensation);

            // Assert
            Assert.IsNull(_actualCompensation, "Nothing should have been saved");
        }
        [TestMethod]
        public void Create_ValidData_NewRecordIsSaved()
        {
            // Arrange
            _employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            _testCompensation = new Compensation() { EmployeeId = _employeeId, Salary = 123.45F, EffectiveDate = DateOnly.Parse("2025-01-15") };

            // Execute
            _compensationService.Create(_testCompensation);

            // Assert
            _actualCompensation = _compensationService.GetByEmployeeId(_employeeId);
            Assert.AreEqual(_actualCompensation.EmployeeId, _testCompensation.EmployeeId, "EmployeeId values should have matched");
            Assert.AreEqual(_actualCompensation.Salary, _testCompensation.Salary, "Salary values should have matched");
            Assert.AreEqual(_actualCompensation.EffectiveDate, _testCompensation.EffectiveDate, "EffectiveDate values should have matched");
            Assert.IsNotNull(_actualCompensation.CompensationId, "CompensationId should have a value");
            Assert.IsNotNull(_actualCompensation.Employee, "Employee should have a value");
        }
        [TestMethod]
        public void Create_InvalidEmployeeID_RecordIsNotSaved()
        {
            // Arrange
            _employeeId = "BAD-ID";
            _testCompensation = new Compensation() { EmployeeId = _employeeId, Salary = 123.45F, EffectiveDate = DateOnly.Parse("2025-01-15") };

            // Execute
            _compensationService.Create(_testCompensation);

            // Assert
            _actualCompensation = _compensationService.GetByEmployeeId(_employeeId);
            Assert.IsNull(_actualCompensation, "Nothing should have been saved");
        }
        [TestMethod]
        public void Create_SalaryIsNegative_RecordIsNotSaved()
        {
            // Arrange
            _employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            _testCompensation = new Compensation() { EmployeeId = _employeeId, Salary = -123.45F, EffectiveDate = DateOnly.Parse("2025-01-15") };

            // Execute
            _compensationService.Create(_testCompensation);

            // Assert
            _actualCompensation = _compensationService.GetByEmployeeId(_employeeId);
            Assert.IsNull(_actualCompensation, "Nothing should have been saved");
        }


    }
}
