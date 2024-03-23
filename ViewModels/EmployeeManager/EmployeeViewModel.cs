using System.Windows;
using System.Windows.Input;
using Microsoft.IdentityModel.Tokens;
using SchedulerDesktop.Commands;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.Services.Api.Interfaces;
using SchedulerDesktop.ViewModels.BaseClasses;

namespace SchedulerDesktop.ViewModels.EmployeeManager;

public class EmployeeViewModel : ViewModelBase
{
    private readonly IEmployeeApiService _employeeApiService;
    private readonly IUserApiService _userApiService;

    private bool _showCallToRegisterFlow;
    public bool ShowCallToRegisterFlow
    {
        get => _showCallToRegisterFlow;
        set => SetField(ref _showCallToRegisterFlow, value);
    }

    public ICommand CallToRegisterCommand { get; }

    public EmployeeViewModel(IEmployeeApiService employeeApiService, IUserApiService userApiService)
    {
        _employeeApiService = employeeApiService;
        _userApiService = userApiService;

        CallToRegisterCommand = new AsyncRelayCommand(CallToRegisterAsync, () => !PhoneNumber.IsNullOrEmpty());
    }

    private bool _isNewEmployee = true;
    private bool IsNewEmployee
    {
        get => _isNewEmployee;
        set
        {
            if (SetField(ref _isNewEmployee, value))
            {
                OnPropertyChanged(nameof(IdChangeEnabled));
            }
        }
    }
    
    public bool IdChangeEnabled => IsNewEmployee;
    
    
    private int _id;
    public int Id
    {
        get => _id;
        set
        {
            if (!IdChangeEnabled) return;
            SetField(ref _id, value);
        }
    }


    private string? _name;
    public string? Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }
    
    
    private double _balance;
    public double Balance
    {
        get => _balance;
        set => SetField(ref _balance, value);
    }
    
    
    private double _difficultBalance;
    public double DifficultBalance
    {
        get => _difficultBalance;
        set => SetField(ref _difficultBalance, value);
    }
    
    
    private bool _active;
    public bool Active
    {
        get => _active;
        set => SetField(ref _active, value);
    }

    public string Role { get; set; } = "Employee";

    private User? _user;
    public User? User
    {
        get => _user;
        set => SetField(ref _user, value);
    }

    private string? _phoneNumber;

    public string? PhoneNumber
    {
        get => _phoneNumber;
        set => SetField(ref _phoneNumber, value);
    }
    
    public void SetContext(Employee? employee, User? user)
    {
        if (employee is null)
        {
            Clear();
            return;
        }

        IsNewEmployee = true;
        Id = employee.Id;
        Name = employee.Name;
        Balance = employee.Balance;
        DifficultBalance = employee.DifficultBalance;
        Active = employee.IsActive;
        User = user;
        PhoneNumber = default;
        ShowCallToRegisterFlow = false;
        IsNewEmployee = false;
    }

    private void Clear()
    {
        IsNewEmployee = true;
        Id = 0;
        Name = null;
        Balance = 0.0;
        DifficultBalance = 0.0;
        Active = true;
        PhoneNumber = default;
        ShowCallToRegisterFlow = false;
    }
    
    
    public bool CanExecute()
    {
        return Id > 0 & !string.IsNullOrEmpty(Name);
    }

    public async Task UpdateEmployee()
    {
        if (IsNewEmployee)
        {
            await PostEmployeeAsync();
        } 
        else
        {
            await PutEmployeeAsync();
        }
    }
    
    private async Task PostEmployeeAsync()
    {
        await _employeeApiService.PostEmployeeAsync(
            new Employee(
                Id, 
                Name!, 
                Balance, 
                DifficultBalance, 
                Active)
            );
    }

    private async Task PutEmployeeAsync()
    {
        await _employeeApiService.PutEmployeeAsync(Id,
            new Employee(
                Id, 
                Name!, 
                Balance, 
                DifficultBalance, 
                Active)
            );
    }

    private async Task<bool> CallToRegisterAsync()
    {
        if (PhoneNumber is null)
        {
            return false;
        }
        
        var result = await _userApiService.CallToRegisterAsync(Id, PhoneNumber);
        if (result)
        {
            MessageBox.Show("נשלח בהצלחה");
        }
        
        PhoneNumber = string.Empty;
        ShowCallToRegisterFlow = false;
        return result;
    }
}
