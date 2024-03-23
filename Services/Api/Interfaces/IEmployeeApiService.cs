using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.Entities;

namespace SchedulerDesktop.Services.Api.Interfaces;

public interface IEmployeeApiService : IApiService<Employee, int, EmployeeDto>
{
    Task<Employee?> GetEmployeeAsync(int key);
    Task<IEnumerable<Employee>?> GetEmployeesAsync();
    Task<IEnumerable<Employee>?> GetActiveEmployeesAsync();
    Task<IEnumerable<Employee>?> GetAssignedEmployeesAsync();
    Task PostEmployeeAsync(Employee employee);
    Task PutEmployeeAsync(int id, Employee employee);
    Task DeleteEmployeeAsync(int id);
}