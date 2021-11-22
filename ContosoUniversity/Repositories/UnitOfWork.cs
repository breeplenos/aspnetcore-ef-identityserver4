using ContosoUniversity.Models;
using ContosoUniversity.Data;
using System.Threading.Tasks;
using System;

namespace ContosoUniversity.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private SchoolContext _context;
        private IRepository<Student> _studentRepo;
        private IRepository<Instructor> _instructorRepo;
        private IRepository<Department> _departmentRepo;
        private IRepository<Course> _courseRepo;
        private IRepository<Enrollment> _enrollmentRepo;
        private IRepository<CourseAssignment> _courseAssignmentRepo;
        private IRepository<OfficeAssignment> _officeAssignmentRepo;

        public UnitOfWork(SchoolContext context)
        {
            context.Database.EnsureCreated();
            _context = context;
        }

        public virtual IRepository<Department> DepartmentRepository
        {
            get => _departmentRepo = _departmentRepo ?? new Repository<Department, SchoolContext>(_context);
        }

        public virtual IRepository<Instructor> InstructorRepository
        {
            get => _instructorRepo = _instructorRepo ?? new Repository<Instructor, SchoolContext>(_context);
        }

        public virtual IRepository<Student> StudentRepository
        {
            get => _studentRepo = _studentRepo ?? new Repository<Student, SchoolContext>(_context);
        }

        public virtual IRepository<Course> CourseRepository
        {
            get => _courseRepo = _courseRepo ?? new Repository<Course, SchoolContext>(_context);
        }

        public virtual IRepository<CourseAssignment> CourseAssignmentRepository
        {
            get => _courseAssignmentRepo = _courseAssignmentRepo ?? new Repository<CourseAssignment, SchoolContext>(_context);
        }

        public virtual IRepository<OfficeAssignment> OfficeAssignmentRepository
        {
            get => _officeAssignmentRepo = _officeAssignmentRepo ?? new Repository<OfficeAssignment, SchoolContext>(_context);
        }

        public virtual IRepository<Enrollment> EnrollmentRepository
        {
            get => _enrollmentRepo = _enrollmentRepo ?? new Repository<Enrollment, SchoolContext>(_context);
        }

        public async Task Commit()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                }
                catch (Exception)
                {
                    _context.Dispose();
                    transaction.Rollback();
                }

            }

        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
