using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Services
{
    public interface IStudentsService
    {
        IQueryable<Student> GetStudents(string sortOrder, string searchString);

        Task<Student> GetStudentDetails(int id);

        Task<Student> GetStudent(int id);

        Task AddStudent(Student student);

        Task UpdateStudent(Student student);

        Task DeleteStudent(int id);

        List<EnrollmentDateGroup> GetStudentsStatistics();
    }
}
