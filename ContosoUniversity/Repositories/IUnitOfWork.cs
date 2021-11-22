using System;
using System.Threading.Tasks;
using ContosoUniversity.Models;

namespace ContosoUniversity.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Department> DepartmentRepository { get; }

        IRepository<Instructor> InstructorRepository { get; }

        IRepository<Student> StudentRepository { get; }

        IRepository<Course> CourseRepository { get; }

        IRepository<CourseAssignment> CourseAssignmentRepository { get; }

        IRepository<OfficeAssignment> OfficeAssignmentRepository { get; }

        IRepository<Enrollment> EnrollmentRepository { get; }

        Task Commit();
    }
}
