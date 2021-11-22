using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Repositories;
using ContosoUniversity.Models;

namespace ContosoUniversity.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly IUnitOfWork _uow;

        public CoursesService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public virtual async Task<Course> GetCourse(int id)
        {
            return await _uow.CourseRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
        }

        public virtual async Task<Course> GetCourseDetails(int id)
        {
            return await _uow.CourseRepository.GetAll()
                .Include(c => c.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Course> GetCourses()
        {
            return _uow.CourseRepository.GetAll()
                .Include(c => c.Department)
                .AsNoTracking();
        }

        public IQueryable<Department> GetDepartments()
        {
            return from d in _uow.DepartmentRepository.GetAll()
                   orderby d.Name
                   select d;
        }

        public virtual async Task AddCourse(Course course)
        {
            await _uow.CourseRepository.AddAsync(course);
            await _uow.Commit();
        }

        public virtual async Task UpdateCourse(Course course)
        {
            course.Department = await _uow.DepartmentRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == course.DepartmentID);

            await _uow.CourseRepository.UpdateAsync(course);
            await _uow.Commit();
        }

        public virtual async Task DeleteCourse(Course course)
        {
            await _uow.CourseRepository.DeleteAsync(course);
            await _uow.Commit();
        }

        public virtual async Task<int> UpdateCourseCredits(int multiplier)
        {
            return await _uow.CourseRepository.ExecuteSqlCommandAsync($"UPDATE Course SET Credits = Credits * {multiplier}");
        }
    }
}
