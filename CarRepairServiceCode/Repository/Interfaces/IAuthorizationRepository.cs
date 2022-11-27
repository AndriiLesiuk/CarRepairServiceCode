using CarRepairServiceCode.Repository.Models;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Repository.Interfaces
{
    public interface IAuthorizationRepository
    {
        Task<Employee> GetEmployeeByCredentials(string login, string pasword);
    }
}
