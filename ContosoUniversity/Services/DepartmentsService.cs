using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Repositories;

namespace ContosoUniversity.Services
{
    public class DepartmentsService : IDepartmentsService
    {
        private readonly IUnitOfWork _uow;

        public DepartmentsService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IQueryable<Department> GetDepartments()
        {
            return _uow.DepartmentRepository.GetAll()
                .Include(d => d.Administrator);
        }

        public virtual async Task<Department> GetDepartment(int id)
        {
            return await _uow.DepartmentRepository.Get(id)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<Department> GetDepartmentDetails(int id)
        {
            return await _uow.DepartmentRepository.Get(id)
                .Include(d => d.Administrator)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public virtual async Task AddDepartment(Department department)
        {
            await _uow.DepartmentRepository.AddAsync(department);
            await _uow.Commit();
        }

        public IQueryable<Instructor> GetInstructors()
        {
            return _uow.InstructorRepository.GetAll();
        }

        public virtual async Task UpdateDepartment(Department department)
        {
            await _uow.DepartmentRepository.UpdateAsync(department);
            await _uow.Commit();
        }

        public virtual async Task DeleteDepartment(Department department)
        {
            await _uow.DepartmentRepository.DeleteAsync(department);
            await _uow.Commit();
        }
    }
}
