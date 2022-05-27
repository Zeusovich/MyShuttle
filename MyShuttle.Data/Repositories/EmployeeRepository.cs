using Microsoft.EntityFrameworkCore;
using MyShuttle.Model;
using System.Linq;
using System.Threading.Tasks;

namespace MyShuttle.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyShuttleContext _context;

        public EmployeeRepository(MyShuttleContext dbcontext)
        {
            _context = dbcontext;
        }

        public async Task<Employee> GetAsync(int employeeId)
        {
            return await _context.Employees
               .Where(e => e.EmployeeId == employeeId)
               .SingleOrDefaultAsync();
        }
    }
}