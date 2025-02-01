using CodeChallenge.Models;
using System.Threading.Tasks;
using System;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetByEmployeeId(String employeeId);
        Compensation Add(Compensation compensation);
        Task SaveAsync();
    }
}
