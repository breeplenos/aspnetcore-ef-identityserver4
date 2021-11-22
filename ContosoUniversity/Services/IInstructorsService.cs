using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Services
{
    public interface IInstructorsService
    {
        Task<InstructorIndexData> GetInstructors(int? id, int? courseID);

        Task<Instructor> GetInstructor(int id);

        Task<Instructor> GetInstructorDetails(int id);

        List<AssignedCourseData> GetAssignedCourses(Instructor instructor);

        Task AddInstructor(Instructor instructor);

        Instructor AddInstructorCourses(Instructor instructor, string[] selectedCourses);

        Instructor UpdateInstructorCourses(Instructor instructor, string[] selectedCourses);

        Task UpdateInstructor(Instructor instructor);

        Task DeleteInstructor(int id);
    }
}
