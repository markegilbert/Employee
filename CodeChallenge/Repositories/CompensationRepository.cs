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
            // TODO: Validate the parameters

            // Ensure that the new Compensation record has a valid ID of its own.
            compensation.CompensationId = Guid.NewGuid().ToString();

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
