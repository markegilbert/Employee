using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext)
        {
            // TODO: Validate these
            _employeeContext = employeeContext;
            _logger = logger;
        }


        public Compensation Add(Compensation compensation)
        {
            // Make sure the Employee actually exists before saving the new compensation record
            if (_employeeContext.Employees.SingleOrDefault(c => c.EmployeeId == compensation.EmployeeId) == null) { return null; }

            _employeeContext.Compensations.Add(compensation);

            return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            // TODO: Return only the most recent record where the effective date is equal to or less than today
            return _employeeContext.Compensations
                .Include(c => c.Employee)
                .SingleOrDefault(c => c.EmployeeId == employeeId);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
