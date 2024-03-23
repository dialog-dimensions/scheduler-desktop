using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Models.Entities;

namespace SchedulerDesktop.Models.DTOs.Entities;

public class EmployeeDto : IDto<Employee, EmployeeDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Balance { get; set; }
    public double DifficultBalance { get; set; }
    public bool Active { get; set; }
    public string Role { get; set; }

    public static EmployeeDto FromEntity(Employee entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Balance = entity.Balance,
        DifficultBalance = entity.DifficultBalance,
        Active = entity.IsActive,
        Role = entity.Role
    };

    public Employee ToEntity() => new(
        Id, 
        Name, 
        Balance, 
        DifficultBalance, 
        Active
        );
}