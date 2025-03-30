namespace EmployeeDirectory.Models;

public class Employee
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;  // Initialize with empty string
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}