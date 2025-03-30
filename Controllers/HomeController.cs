using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeDirectory.Models;
using EmployeeDirectory.Services; // Add this using directive

namespace EmployeeDirectory.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly EmployeeService _employeeService;

    // Inject both logger and EmployeeService
    public HomeController(ILogger<HomeController> logger, EmployeeService employeeService)
    {
        _logger = logger;
        _employeeService = employeeService;
    }

    public IActionResult Index()
    {
        // Get employees from the service and pass to view
        var employees = _employeeService.GetAll();
        return View(employees);
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
}