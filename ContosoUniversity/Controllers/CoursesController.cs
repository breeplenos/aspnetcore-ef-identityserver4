using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Authorization;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICoursesService _courseService;

        public CoursesController(ICoursesService courseService)
        {
            _courseService = courseService;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            return View(await _courseService.GetCourses().ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseService.GetCourseDetails(id.Value);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        // POST: Courses/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseNumber,Credits,DepartmentID,Title")] Course course)
        {
            if (ModelState.IsValid)
            {
                await _courseService.AddCourse(course);
                return RedirectToAction(nameof(Index));
            }

            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseService.GetCourse(id.Value);

            if (course == null)
            {
                return NotFound();
            }

            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // POST: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseToUpdate = await _courseService.GetCourseDetails(id.Value);

            if (await TryUpdateModelAsync<Course>(courseToUpdate, "", c => c.Credits, c => c.DepartmentID, c => c.Title))
            {
                try
                {
                    await _courseService.UpdateCourse(courseToUpdate);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator");
                }
            }

            PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);
            return View(courseToUpdate);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseService.GetCourseDetails(id.Value);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseService.GetCourseDetails(id.Value);

            if (course == null)
            {
                return NotFound();
            }

            await _courseService.DeleteCourse(course);
            return RedirectToAction(nameof(Index));
        }

        // POST: Courses/UpdateCourseCredits/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateCourseCredits(int? multiplier)
        {
            if (multiplier != null)
            {
                ViewData["RowsAffected"] = await _courseService.UpdateCourseCredits(multiplier.Value);
            }

            return View();
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            ViewBag.DepartmentID = new SelectList(_courseService.GetDepartments().AsNoTracking(), "ID", "Name", selectedDepartment);
        }
    }
}
