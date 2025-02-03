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
            if (logger == null) { throw new ArgumentNullException($"The parameter '{nameof(logger)}' was null or otherwise invalid"); }
            if (employeeContext == null) { throw new ArgumentNullException($"The parameter '{nameof(employeeContext)}' was null or otherwise invalid"); }

            _employeeContext = employeeContext;
            _logger = logger;
        }


        public Compensation Add(Compensation compensation)
        {
            // Make sure the Employee actually exists before saving the new compensation record
            if (_employeeContext.Employees.SingleOrDefault(c => c.EmployeeId == compensation.EmployeeId) == null) { return null; }


            // TODO: Currently, this method will allow duplicates by Employee ID / Effective Date.  Consider extending this to an upsert
            //       where if the Employee ID and Effective Date of the new compensation record matches one that is already on file, update
            //       it; otherwise add the new one.


            _employeeContext.Compensations.Add(compensation);

            return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            // Return only the most recent record where the effective date is equal to or less than today
            return _employeeContext.Compensations
                .Include(c => c.Employee)
                .Where(c => c.EffectiveDate <= DateOnly.FromDateTime(DateTime.Today) && c.EmployeeId == employeeId)
                .OrderByDescending(c => c.EffectiveDate)
                .FirstOrDefault();
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
