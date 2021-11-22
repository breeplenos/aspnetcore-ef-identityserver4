using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using System.Threading.Tasks;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Authorization;

namespace ContosoUniversity.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentsService _studentService;

        public StudentsController(IStudentsService studentService)
        {
            _studentService = studentService;
        }

        // GET: Students
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "LastName_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "EnrollmentDate" ? "EnrollmentDate_desc" : "EnrollmentDate";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var students = _studentService.GetStudents(sortOrder, searchString);
            int pageSize = 3;
            return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentService.GetStudentDetails(id.Value);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstMidName, LastName, EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _studentService.AddStudent(student);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentService.GetStudent(id.Value);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentToUpdate = await _studentService.GetStudentDetails(id.Value);

            if (await TryUpdateModelAsync<Student>(studentToUpdate, "", s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                try
                {
                    await _studentService.UpdateStudent(studentToUpdate);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator");
                }
            }

            return View(studentToUpdate);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _studentService.GetStudent(id.Value);

            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again, and if the problem persists see your system administrator";
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _studentService.DeleteStudent(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
