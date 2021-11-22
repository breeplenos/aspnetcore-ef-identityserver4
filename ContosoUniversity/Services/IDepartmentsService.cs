using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;

namespace ContosoUniversity.Services
{
    public interface IDepartmentsService
    {
        IQueryable<Department> GetDepartments();

        Task<Department> GetDepartmentDetails(int id);

        Task<Department> GetDepartment(int id);

        Task AddDepartment(Department department);

        IQueryable<Instructor> GetInstructors();

        Task UpdateDepartment(Department department);

        Task DeleteDepartment(Department department);
    }
}
