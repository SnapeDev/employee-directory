using EmployeeDirectory.Models;
using System.Text.Json;
namespace EmployeeDirectory.Services;

public class EmployeeService
{
    // Session management components
    private readonly IHttpContextAccessor _httpAccessor;  
    private const string SessionKey = "UserEmployees";  // Session key for employee data

    
    // Contains base employees that persist across sessions
    private readonly List<Employee> _globalTemplateEmployees = new()
    {
        
        new Employee { ID = 1, Name = "Default Admin", Position = "Admin", Department = "IT" }
    };

    // Constructor to inject IHttpContextAccessor for session access
    public EmployeeService(IHttpContextAccessor httpAccessor)
    {
        _httpAccessor = httpAccessor;  
    }

    // Retrieve user-specific employee list from session
    private List<Employee> GetUserEmployees()
    {
        var session = _httpAccessor.HttpContext!.Session;
        var json = session.GetString(SessionKey);         
        
        // Initialize new user session
        if (string.IsNullOrEmpty(json))
        {
            
            // - Test user for new sessions
            var newUserList = new List<Employee>(_globalTemplateEmployees)
            {
                new Employee { ID = 2, Name = "Michael Chambers", Position = "Staff", Department = "HR" }
            };
            
            session.SetString(SessionKey, JsonSerializer.Serialize(newUserList));  // Store initial data
            return newUserList;
        }

        return JsonSerializer.Deserialize<List<Employee>>(json)!;  // Return existing data
    }

    // Persist modified employee list to session
    private void SaveUserEmployees(List<Employee> employees)
    {
        _httpAccessor.HttpContext!.Session.SetString(
            SessionKey, 
            JsonSerializer.Serialize(employees)  
        );
    }
    
    // Get complete employee list for current user
    public List<Employee> GetAll() => GetUserEmployees();

    // Add new employee with auto-incremented ID
    public void Add(Employee employee)
    {
        var userEmployees = GetUserEmployees();
        employee.ID = userEmployees.Max(e => e.ID) + 1; 
        userEmployees.Add(employee);
        SaveUserEmployees(userEmployees);  
    }

    // Remove employee by ID
    public void Delete(int id)
    {
        var userEmployees = GetUserEmployees();
        userEmployees.RemoveAll(e => e.ID == id);  
        SaveUserEmployees(userEmployees);         
    }
}