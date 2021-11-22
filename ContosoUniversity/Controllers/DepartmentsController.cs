using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.AspNetCore.Authorization;

namespace ContosoUniversity.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentsService _departmentService;

        public DepartmentsController(IDepartmentsService departmentsService)
        {
            _departmentService = departmentsService;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            return View(await _departmentService.GetDepartments().ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _departmentService.GetDepartmentDetails(id.Value);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var department = new Department();
            PopulateInstructorData(department);
            return View();
        }

        // POST: Departments/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,Name,Budget,StartDate,InstructorID,RowVersion")] Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.AddDepartment(department);
                return RedirectToAction(nameof(Index));
            }

            PopulateInstructorData(department);
            return View(department);
        }

        // GET: Departments/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _departmentService.GetDepartmentDetails(id.Value);

            if (department == null)
            {
                return NotFound();
            }

            PopulateInstructorData(department);
            return View(department);
        }

        // POST: Departments/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentToUpdate = await _departmentService.GetDepartmentDetails(id.Value);

            if (departmentToUpdate == null)
            {
                Department deletedDepartment = new Department();

                await TryUpdateModelAsync(deletedDepartment);
                ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");

                PopulateInstructorData(deletedDepartment);
                return View(deletedDepartment);
            }

            if (await TryUpdateModelAsync<Department>(departmentToUpdate, "", s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorID))
            {
                try
                {
                    departmentToUpdate.RowVersion = rowVersion;
                    await _departmentService.UpdateDepartment(departmentToUpdate);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Department)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");

                        if (databaseValues.Budget != clientValues.Budget)
                            ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget:c}");

                        if (databaseValues.StartDate != clientValues.StartDate)
                            ModelState.AddModelError("StartDate", $"Current value: {databaseValues.StartDate:d}");

                        if (databaseValues.InstructorID != clientValues.InstructorID)
                        {
                            Instructor databaseInstructor = await _departmentService.GetInstructors().FirstOrDefaultAsync(i => i.ID == databaseValues.InstructorID);
                            ModelState.AddModelError("InstructorID", $"Current value: {databaseInstructor?.FullName}");
                        }

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you got the original value. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to edit this record, click "
                                + "the Save button again. Otherwise click the Back to List hyperlink.");
                        departmentToUpdate.RowVersion = (byte[])databaseValues.RowVersion;
                    }
                }
            }

            PopulateInstructorData(departmentToUpdate);
            return View(departmentToUpdate);
        }

        // GET: Departments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _departmentService.GetDepartmentDetails(id.Value);

            if (department == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction(nameof(Index));
                }

                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetDepartment(id);

            if (department == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _departmentService.DeleteDepartment(department);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = department.ID });
            }
        }

        private void PopulateInstructorData(Department department)
        {
            ViewData["InstructorID"] = new SelectList(_departmentService.GetInstructors(), "ID", "FullName", department.InstructorID);
        }
    }
}
