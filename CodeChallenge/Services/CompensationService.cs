using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;


        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            if (logger == null) { throw new ArgumentNullException($"The parameter '{nameof(logger)}' was null or otherwise invalid"); }
            if (compensationRepository == null) { throw new ArgumentNullException($"The parameter '{nameof(compensationRepository)}' was null or otherwise invalid"); }

            _compensationRepository = compensationRepository;
            _logger = logger;
        }



        public Compensation Create(Compensation compensation)
        {
            if (compensation == null) { return null; }
            if (compensation.Salary < 0) { return null; }

            // Ensure that the new Compensation record has a valid ID of its own before trying to save it.
            compensation.CompensationId = Guid.NewGuid().ToString();
            if (_compensationRepository.Add(compensation) == null) { return null; }

            _compensationRepository.SaveAsync().Wait();

            return compensation;
        }


        public Compensation GetByEmployeeId(string employeeId)
        {
            if (!String.IsNullOrEmpty(employeeId))
            {
                return _compensationRepository.GetByEmployeeId(employeeId);
            }

            return null;
        }
    }
}
