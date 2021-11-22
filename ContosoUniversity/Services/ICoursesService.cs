using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;

namespace ContosoUniversity.Services
{
    public interface ICoursesService
    {
        IQueryable<Course> GetCourses();

        Task<Course> GetCourse(int id);

        Task<Course> GetCourseDetails(int id);

        IQueryable<Department> GetDepartments();

        Task AddCourse(Course course);

        Task UpdateCourse(Course course);

        Task DeleteCourse(Course course);

        Task<int> UpdateCourseCredits(int multiplier);
    }
}
