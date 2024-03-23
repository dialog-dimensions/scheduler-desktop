using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.ViewModels.BaseClasses;

namespace SchedulerDesktop.ViewModels.ScheduleManager;

public class ShiftViewModel : ViewModelBase
{
    public Shift? Shift { get; private set; }
    public bool Initialized { get; private set; }
    
    public void Initialize(Shift shift)
    {
        SetContext(shift);
        Initialized = true;
    }
    
    private void SetContext(Shift shift)
    {
        Shift = shift;
    }
    
    private Employee? _employee;
    
    public Employee? Employee
    {
        get => _employee;
        set
        {
            if (!Initialized) return;
            if (SetField(ref _employee, value))
            {
                Shift!.Employee = value;
            }
        }
    }
}
