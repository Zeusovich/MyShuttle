using MyShuttle.Model;
using System.Threading.Tasks;

namespace MyShuttle.Data
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetAsync(int employeeId);
    }
}