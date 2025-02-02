using CodeChallenge.Data;
using CodeChallenge.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;


namespace CodeCodeChallenge.Tests.Unit
{
    [TestClass]
    public class CompensationRepositoryTests
    {
        private ArgumentNullException _argumentNullException;
        private ILogger<ICompensationRepository> _logger;
        private EmployeeContext _employeeContext;


        [TestInitialize]
        public void TestInitialize()
        {
            _argumentNullException = null;
            _logger = Substitute.For<ILogger<ICompensationRepository>>();

            // This does not establishing a fully functional EmployeeContext; for the current tests, I only need
            // a valid object of this type.  For a fully functional version, please see CompensationServiceTests
            _employeeContext = new EmployeeContext(new DbContextOptionsBuilder<EmployeeContext>().Options);
        }



        [TestMethod]
        public void InstantiateClass_LoggerIsNull_ExceptionThrown()
        {
            _argumentNullException = Assert.ThrowsException<ArgumentNullException>(() => new CompensationRepository(null, _employeeContext));
            Assert.IsTrue(_argumentNullException.Message.Contains("'logger'"), "Exception didn't reference the correct parameter");
        }
        [TestMethod]
        public void InstantiateClass_EmployeeContextIsNull_ExceptionThrown()
        {
            _argumentNullException = Assert.ThrowsException<ArgumentNullException>(() => new CompensationRepository(_logger, null));
            Assert.IsTrue(_argumentNullException.Message.Contains("'employeeContext'"), "Exception didn't reference the correct parameter");
        }

    }
}
