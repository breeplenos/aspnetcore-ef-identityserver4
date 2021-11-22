using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Authorization;

namespace ContosoUniversity.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly IInstructorsService _instructorService;

        public InstructorsController(IInstructorsService instructorService)
        {
            _instructorService = instructorService;
        }

        // GET: Instructors
        public async Task<IActionResult> Index(int? id, int? courseID)
        {
            var viewModel = await _instructorService.GetInstructors(id, courseID);

            if (id != null)
                ViewData["InstructorID"] = id.Value;

            if (courseID != null)
                ViewData["CourseID"] = courseID.Value;

            return View(viewModel);
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _instructorService.GetInstructor(id.Value);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();
            PopulateAssignedCourseData(instructor);
            return View();
        }

        // POST: Instructors/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstMidName,HireDate,LastName,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            instructor = _instructorService.AddInstructorCourses(instructor, selectedCourses);

            if (ModelState.IsValid)
            {
                await _instructorService.AddInstructor(instructor);
                return RedirectToAction(nameof(Index));
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _instructorService.GetInstructorDetails(id.Value);

            if (instructor == null)
            {
                return NotFound();
            }

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            ViewData["Courses"] = _instructorService.GetAssignedCourses(instructor);
        }

        // POST: Instructors/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorToUpdate = await _instructorService.GetInstructorDetails(id.Value);

            if (await TryUpdateModelAsync<Instructor>(instructorToUpdate,"", i => i.FirstMidName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment))
            {
                if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                    instructorToUpdate.OfficeAssignment = null;

                _instructorService.UpdateInstructorCourses(instructorToUpdate, selectedCourses);

                try
                {
                    await _instructorService.UpdateInstructor(instructorToUpdate);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            _instructorService.UpdateInstructorCourses(instructorToUpdate, selectedCourses);
            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }

        // GET: Instructors/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _instructorService.GetInstructor(id.Value);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _instructorService.DeleteInstructor(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
