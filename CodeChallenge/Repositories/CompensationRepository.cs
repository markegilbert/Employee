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
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            // TODO: Validate these
            _compensationContext = compensationContext;
            _logger = logger;
        }


        public Compensation Add(Compensation compensation)
        {
            // TODO: Validate the parameters

            // Ensure that the new Compensation record has a valid ID of its own.
            compensation.CompensationId = Guid.NewGuid().ToString();
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            // TODO: Return only the most recent record where the effective date is equal to or less than today
            return _compensationContext.Compensations
                .Include(c => c.Employee)
                .SingleOrDefault(c => c.Employee.EmployeeId == employeeId);
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
