using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using ContosoUniversity.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly IUnitOfWork _uow;

        public StudentsService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IQueryable<Student> GetStudents(string sortOrder, string searchString)
        {
            var students = from s in _uow.StudentRepository.GetAll()
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Trim().ToLower().Contains(searchString.Trim().ToLower())
                                       || s.FirstMidName.Trim().ToLower().Contains(searchString.Trim().ToLower()));
            }

            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "LastName";
            }

            bool descending = false;

            if (sortOrder.EndsWith("_desc"))
            {
                sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                descending = true;
            }

            if (descending)
            {
                students = students.OrderByDescending(s => EF.Property<Student>(s, sortOrder));
            }
            else
            {
                students = students.OrderBy(s => EF.Property<Student>(s, sortOrder));
            }

            return students;
        }

        public virtual async Task<Student> GetStudent(int id)
        {
            return await _uow.StudentRepository.Get(id)
                 .AsNoTracking()
                 .SingleOrDefaultAsync();
        }

        public virtual async Task<Student> GetStudentDetails(int id)
        {
            return await _uow.StudentRepository.Get(id)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public virtual async Task AddStudent(Student student)
        {
            await _uow.StudentRepository.AddAsync(student);
            await _uow.Commit();
        }

        public virtual async Task UpdateStudent(Student student)
        {
            await _uow.StudentRepository.UpdateAsync(student);
            await _uow.Commit();
        }

        public virtual async Task DeleteStudent(int id)
        {
            var student = await GetStudent(id);
            if (student != null)
            {
                await _uow.StudentRepository.DeleteAsync(student);
                await _uow.Commit();
            }
        }

        public List<EnrollmentDateGroup> GetStudentsStatistics()
        {
            var students = from s in _uow.StudentRepository.GetAll()
                               group s by s.EnrollmentDate into dateGroup
                               select new EnrollmentDateGroup()
                               {
                                   EnrollmentDate = dateGroup.Key,
                                   StudentCount = dateGroup.Count()
                               };

            return students.ToList();
        }
    }
}
