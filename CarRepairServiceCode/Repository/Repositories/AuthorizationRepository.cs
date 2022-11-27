using CarRepairServiceCode.Repository.Contexts;
using CarRepairServiceCode.Repository.Interfaces;
using CarRepairServiceCode.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Repository.Repositories
{
    public class AuthorizationRepository : BaseRepository<CarRepairServiceDB_Context>, IAuthorizationRepository
    {
        public AuthorizationRepository(CarRepairServiceDB_Context context) : base(context)
        {
        }

        public async Task<Employee> GetEmployeeByCredentials(string login, string pasword)
        {
            var emp = await _context.Employees.FirstOrDefaultAsync(x => x.EmpLogin.ToLower() == login.ToLower() && x.EmpPassword == pasword);
            return emp;
        }
    }
}
