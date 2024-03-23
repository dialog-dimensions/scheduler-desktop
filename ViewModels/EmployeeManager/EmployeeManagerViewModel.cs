using System.Windows.Input;
using SchedulerDesktop.Commands;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.Services.Api.Interfaces;
using SchedulerDesktop.ViewModels.BaseClasses;

namespace SchedulerDesktop.ViewModels.EmployeeManager;

public class EmployeeManagerViewModel : ViewModelBase
{
    private readonly IEmployeeApiService _apiService;
    private readonly IUserApiService _userApiService;
    
    public EmployeeViewModel EmployeeViewModel { get; }
    
    public ICommand ExecuteCommand { get; }
    public ICommand ClearEmployeeCommand { get; }
    public ICommand DeleteEmployeeCommand { get; }
    public ICommand CallToRegisterCommand { get; }

    public EmployeeManagerViewModel(IEmployeeApiService apiService, EmployeeViewModel employeeViewModel,
        IUserApiService userApiService)
    {
        _apiService = apiService;
        _userApiService = userApiService;
        EmployeeViewModel = employeeViewModel;
        ExecuteCommand = new AsyncRelayCommand(Execute, CanExecute);
        ClearEmployeeCommand = new RelayCommand(ClearEmployee, CanClearEmployee);
        DeleteEmployeeCommand = new AsyncRelayCommand(DeleteEmployee, CanDeleteEmployee);
        CallToRegisterCommand = EmployeeViewModel.CallToRegisterCommand;
    }
    
    
    private IEnumerable<Employee> _employees = new List<Employee>();
    public IEnumerable<Employee> Employees
    {
        get => _employees;
        set => SetField(ref _employees, value);
    }

    public IEnumerable<Employee> AssignedEmployees { get; set; } = new List<Employee>();


    private Employee? _selectedEmployee;
    
    public Employee? SelectedEmployee
    {
        get => _selectedEmployee;
        set
        {
            if (SetField(ref _selectedEmployee, value))
            {
                var user = Users.FirstOrDefault(u => u.Id == value?.Id.ToString());
                EmployeeViewModel.SetContext(value, user);
            }
        }
    }

    private IEnumerable<User> _users = new List<User>();

    public IEnumerable<User> Users
    {
        get => _users;
        set => SetField(ref _users, value);
    }

    public async Task RefreshEmployees()
    {
        var employees = await _apiService.GetEmployeesAsync();
        var assignedEmployees = await _apiService.GetAssignedEmployeesAsync();
        var users = await _userApiService.GetUsersAsync();
        
        Employees = employees ?? new List<Employee>();
        AssignedEmployees = assignedEmployees ?? new List<Employee>();
        Users = users ?? new List<User>();
        if (!(SelectedEmployee is null || Employees.Contains(SelectedEmployee)))
        {
            SelectedEmployee = default;
        }
    }

    private bool CanExecute()
    {
        return EmployeeViewModel.CanExecute();
    }

    private async Task Execute()
    {
        await EmployeeViewModel.UpdateEmployee();
        await RefreshEmployees();
    }

    private bool CanClearEmployee(object? parameter)
    {
        return true;
    }

    private void ClearEmployee(object? parameter)
    {
        SelectedEmployee = null;
    }

    private bool CanDeleteEmployee()
    {
        return SelectedEmployee is not null && !AssignedEmployees.Contains(SelectedEmployee);
    }

    private async Task DeleteEmployee()
    {
        await _apiService.DeleteEmployeeAsync(SelectedEmployee!.Id);
        await RefreshEmployees();
    }
}
