using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Repositories;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Services
{
    public class InstructorsService : IInstructorsService
    {
        private readonly IUnitOfWork _uow;

        public InstructorsService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public virtual async Task<InstructorIndexData> GetInstructors(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = await _uow.InstructorRepository.GetAll()
                  .Include(i => i.OfficeAssignment)
                  .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Enrollments)
                            .ThenInclude(i => i.Student)
                  .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Department)
                  .AsNoTracking()
                  .OrderBy(i => i.LastName)
                  .ToListAsync();

            if (id != null)
            {
                Instructor instructor = viewModel.Instructors.Where(i => i.ID == id.Value).Single();
                viewModel.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if (courseID != null)
                viewModel.Enrollments = viewModel.Courses.Where(x => x.ID == courseID).Single().Enrollments;

            return viewModel;
        }

        public virtual async Task<Instructor> GetInstructor(int id)
        {
            return await _uow.InstructorRepository.Get(id)
                .SingleOrDefaultAsync();
        }

        public virtual async Task<Instructor> GetInstructorDetails(int id)
        {
            return await _uow.InstructorRepository.Get(id)
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
        }

        public List<AssignedCourseData> GetAssignedCourses(Instructor instructor)
        {
            var allCourses = _uow.CourseRepository.GetAll();
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();

            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.ID,
                    CourseNumber = course.CourseNumber,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.ID)
                });
            }

            return viewModel;
        }

        public Instructor AddInstructorCourses(Instructor instructor, string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.CourseAssignments = new List<CourseAssignment>();

                foreach (var course in selectedCourses)
                {
                    var courseToAdd = new CourseAssignment { InstructorID = instructor.ID, CourseID = int.Parse(course) };
                    instructor.CourseAssignments.Add(courseToAdd);
                }
            }

            return instructor;
        }

        public virtual async Task AddInstructor(Instructor instructor)
        {
            await _uow.InstructorRepository.AddAsync(instructor);
            await _uow.Commit();
        }

        public Instructor UpdateInstructorCourses(Instructor instructor, string[] selectedCourses)
        {
            if (selectedCourses == null)
            {
                instructor.CourseAssignments = new List<CourseAssignment>();
                return instructor;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.Course.ID));

            foreach (var course in _uow.CourseRepository.GetAll().AsNoTracking())
            {
                if (selectedCoursesHS.Contains(course.ID.ToString()))
                {
                    if (!instructorCourses.Contains(course.ID))
                    {
                        var courseToAdd = new CourseAssignment { InstructorID = instructor.ID, CourseID = course.ID };
                        instructor.CourseAssignments.Add(courseToAdd);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.ID))
                    {
                        CourseAssignment courseToRemove = instructor.CourseAssignments.FirstOrDefault(i => i.CourseID == course.ID);
                        _uow.CourseAssignmentRepository.DeleteAsync(courseToRemove);
                    }
                }
            }

            return instructor;
        }

        public virtual async Task UpdateInstructor(Instructor instructor)
        {
            await _uow.InstructorRepository.UpdateAsync(instructor);
            await _uow.Commit();
        }

        public virtual async Task DeleteInstructor(int id)
        {
            Instructor instructor = await _uow.InstructorRepository.GetAll()
                .Include(i => i.CourseAssignments)
                .SingleAsync(i => i.ID == id);

            var departments = await _uow.DepartmentRepository.GetAll()
                .Where(d => d.InstructorID == id)
                .ToListAsync();
            departments.ForEach(d => d.InstructorID = null);

            await _uow.InstructorRepository.DeleteAsync(instructor);
            await _uow.Commit();
        }
    }
}
