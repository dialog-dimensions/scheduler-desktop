using System.Net.Http;
using System.Windows.Input;
using SchedulerDesktop.Commands;
using SchedulerDesktop.Enums;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.Services.Api.Interfaces;
using SchedulerDesktop.ViewModels.BaseClasses;

namespace SchedulerDesktop.ViewModels.ExceptionsManager;

public class ExceptionsManagerViewModel : ViewModelBase
{
    private readonly IShiftExceptionApiService _apiService;
    private readonly IScheduleApiService _scheduleApiService;
    private readonly IEmployeeApiService _employeeApiService;

    public ICommand SaveExceptionCommand { get; }

    public bool Initialized { get; private set; }

    public ExceptionsManagerViewModel(IShiftExceptionApiService apiService, IScheduleApiService scheduleApiService, 
        IEmployeeApiService employeeApiService)
    {
        _apiService = apiService;
        _scheduleApiService = scheduleApiService;
        _employeeApiService = employeeApiService;
        SaveExceptionCommand = new AsyncRelayCommand(SaveException, CanSaveException);
    }

    public async Task Initialize()
    {
        Schedules = await _scheduleApiService.GetFlatSchedulesAsync() ??
                    throw new HttpRequestException("Unable to retrieve schedules from database.");
        Employees = await _employeeApiService.GetEmployeesAsync() ??
                    throw new HttpRequestException("Unable to retrieve employees from database.");
        Initialized = true;
    }
    
    private IEnumerable<FlatSchedule> _schedules = new List<FlatSchedule>();

    public IEnumerable<FlatSchedule> Schedules
    {
        get => _schedules;
        set
        {
            if (SetField(ref _schedules, value))
            {

            }
        }
    }

    private IEnumerable<Shift> _shifts = new List<Shift>();

    public IEnumerable<Shift> Shifts
    {
        get => _shifts;
        set
        {
            if (SetField(ref _shifts, value))
            {

            }
        }
    }

    private IEnumerable<Employee> _employees = new List<Employee>();

    public IEnumerable<Employee> Employees
    {
        get => _employees;
        set => SetField(ref _employees, value);
    }

    public static IEnumerable<ExceptionType> ExceptionTypes => Enumerable.Range(1, 3).Select(i => (ExceptionType)i);


    private FlatSchedule? _schedule;
    public FlatSchedule? Schedule
    {
        get => _schedule;
        set
        {
            if (SetField(ref _schedule, value))
            {

            }
        }
    }

    private Shift? _shift;
    public Shift? Shift
    {
        get => _shift;
        set
        {
            if (SetField(ref _shift, value))
            {

            }
        }
    }

    private Employee? _employee;
    public Employee? Employee
    {
        get => _employee;
        set
        {
            if (SetField(ref _employee, value))
            {

            }
        }
    }

    private ExceptionType? _exceptionType;
    public ExceptionType? ExceptionType
    {
        get => _exceptionType;
        set
        {
            if (SetField(ref _exceptionType, value))
            {

            }
        }
    }

    private string _reason = string.Empty;
    public string Reason
    {
        get => _reason;
        set => SetField(ref _reason, value);
    }

    public async Task LoadShifts(DateTime scheduleKey)
    {
        Shifts = await _scheduleApiService.GetScheduleAsync(scheduleKey) ??
                 throw new HttpRequestException("Unable to load shifts.");
    }

    private async Task SaveException()
    {
        await _apiService.PostExceptionAsync(
            new ShiftException(ExceptionType!.Value, Shift!.StartDateTime, Employee!.Id, Reason));
    } 
    
    private bool CanSaveException()
    {
        return Schedule is not null &&
               Shift is not null &&
               Employee is not null &&
               ExceptionType is not null &&
               (ExceptionType != Enums.ExceptionType.Constraint || !string.IsNullOrEmpty(Reason));
    }
}