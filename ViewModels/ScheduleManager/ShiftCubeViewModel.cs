using SchedulerDesktop.Enums;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.Models.Items;
using SchedulerDesktop.ViewModels.BaseClasses;

namespace SchedulerDesktop.ViewModels.ScheduleManager;

public class ShiftCubeViewModel : ViewModelBase
{
    public ShiftViewModel ShiftViewModel { get; private set; }
    private readonly ScheduleDisplayViewModel _displayViewModel;
    
    public bool Initialized { get; private set; }
    

    private IEnumerable<EmployeeItem> _employeeItems = new List<EmployeeItem>();
    public IEnumerable<EmployeeItem> EmployeeItems
    {
        get => _employeeItems;
        private set
        {
            var list = value.ToList();
            list.Add(new EmployeeItem(Employee.CreateBlank()));
            list = list
                .OrderBy(item => item.ExceptionType)
                .ThenBy(item => item.Employee.Name)
                .ToList();
            SetField(ref _employeeItems, list);
        }
    }


    private EmployeeItem? _selectedItem;

    public EmployeeItem? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (SetField(ref _selectedItem, value))
            {
                ShiftViewModel.Employee = value?.Employee;
            }
        }
    }

    public ShiftCubeViewModel(ScheduleDisplayViewModel displayViewModel, ShiftViewModel shiftViewModel)
    {
        _displayViewModel = displayViewModel;
        ShiftViewModel = shiftViewModel;
    }

    public void Initialize(Shift shift)
    {
        ShiftViewModel.Initialize(shift);
        InitializeItems();
        if (shift.EmployeeId is not null)
        {
            SelectedItem = EmployeeItems.First(item => item.Employee.Id == shift.EmployeeId);
        }
        Initialized = true;
    }

    private void InitializeItems()
    {
        var employees = _displayViewModel.Employees!;
        EmployeeItems = employees.Select(emp => new EmployeeItem(emp, EmployeeException(emp)));
    }

    public bool SelectEmployee(int employeeId)
    {
        var item = EmployeeItems.FirstOrDefault(item => item.Employee.Id == employeeId);
        if (item is null)
        {
            return false;
        }

        SelectedItem = item;
        return true;
    }
    
    private ShiftException? EmployeeException(Employee employee)
    {
        return _displayViewModel.Exceptions!
            .Where(ex => ex.ShiftKey == ShiftViewModel.Shift!.StartDateTime)
            .FirstOrDefault(ex => ex.EmployeeId == employee.Id);
    }

    public bool ConstraintsIndication => _displayViewModel.Exceptions?
        .Where(ex => ex.ExceptionType == ExceptionType.Constraint)
        .Any(ex => ex.ShiftKey == ShiftViewModel.Shift?.StartDateTime) ?? false;
    
    public bool OffPreferencesIndication => _displayViewModel.Exceptions?
        .Where(ex => ex.ExceptionType == ExceptionType.OffPreference)
        .Any(ex => ex.ShiftKey == ShiftViewModel.Shift?.StartDateTime) ?? false;
    
    public bool OnPreferencesIndication => _displayViewModel.Exceptions? 
        .Where(ex => ex.ExceptionType == ExceptionType.OnPreference)
        .Any(ex => ex.ShiftKey == ShiftViewModel.Shift?.StartDateTime) ?? false;
}
