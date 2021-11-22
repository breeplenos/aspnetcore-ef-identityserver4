using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ContosoUniversity.Services;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStudentsService _studentService;

        public HomeController(IStudentsService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View(_studentService.GetStudentsStatistics());
        }
    }
}
