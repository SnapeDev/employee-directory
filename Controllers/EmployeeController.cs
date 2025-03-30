using Microsoft.AspNetCore.Mvc;
using EmployeeDirectory.Models;
using EmployeeDirectory.Services;
using System.Linq;

namespace EmployeeDirectory.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromBody] Employee employee)  // Changed to accept JSON
        {
            if (ModelState.IsValid)
            {
                _employeeService.Add(employee);
                return Json(new { 
                    success = true, 
                    message = "Employee added successfully!" 
                });
            }

            // Return validation errors
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
                
            return Json(new { 
                success = false, 
                errors = errors 
            });
        }

        

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _employeeService.Delete(id);
            return Json(new { 
                success = true, 
                message = "Employee deleted successfully!" 
            });
        }
    }
}