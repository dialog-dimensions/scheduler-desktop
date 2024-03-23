using System.Windows.Input;
using SchedulerDesktop.Commands;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.Services.Api.Interfaces;
using SchedulerDesktop.ViewModels.BaseClasses;

namespace SchedulerDesktop.ViewModels.ScheduleManager;

public class ScheduleDashboardViewModel : ViewModelBase
{
    private readonly IScheduleApiService _apiService;
    public ScheduleDisplayViewModel Display { get; }

    
    private IEnumerable<FlatSchedule> _schedules = new List<FlatSchedule>();
    public IEnumerable<FlatSchedule> Schedules
    {
        get => _schedules;
        set => SetField(ref _schedules, value);
    }

    private FlatSchedule? _selectedSchedule;
    public FlatSchedule? SelectedSchedule
    {
        get => _selectedSchedule;
        set => SetField(ref _selectedSchedule, value);
    }

    public ICommand SelectNearestCommand { get; }
    public ICommand SelectNearestIncompleteCommand { get; }
    public ICommand CommitCommand { get; }
    public ICommand CommitAndExportCommand { get; }
    public ICommand RevertCommand { get; }
    public ICommand RunSchedulerCommand { get; }
    

    public ScheduleDashboardViewModel(IScheduleApiService apiService, ScheduleDisplayViewModel display)
    {
        _apiService = apiService;
        Display = display;
        SelectNearestCommand = new RelayCommand(SelectNearest, CanSelectNearest);
        SelectNearestIncompleteCommand = new RelayCommand(SelectNearestIncomplete, CanSelectNearestIncomplete);
        CommitCommand = Display.CommitCommand;
        CommitAndExportCommand = Display.CommitAndExportCommand;
        RunSchedulerCommand = Display.RunSchedulerCommand;
        RevertCommand = Display.RevertCommand;
    }

    public async Task LoadSchedulesAsync()
    {
        Schedules = await _apiService.GetFlatSchedulesAsync() ?? new List<FlatSchedule>();
    }

    public async Task UpdateDisplayAsync()
    {
        await Display.InitializeAsync(SelectedSchedule!.StartDateTime);
    }
    
    private void SelectNearest(object? parameter)
    {
        SelectedSchedule = Schedules
            .Where(sch => sch.StartDateTime > DateTime.Now)
            .MinBy(sch => sch.StartDateTime)!;
    }

    private bool CanSelectNearest(object? parameter)
    {
        return Schedules.Any(sch => sch.StartDateTime > DateTime.Now);
    }

    private void SelectNearestIncomplete(object? parameter)
    {
        SelectedSchedule = Schedules
            .Where(sch => sch.StartDateTime > DateTime.Now)
            .Where(sch => !sch.IsFullyScheduled)
            .MinBy(sch => sch.StartDateTime)!;
    }

    private bool CanSelectNearestIncomplete(object? parameter)
    {
        return Schedules
            .Where(sch => sch.StartDateTime > DateTime.Now)
            .Any(sch => !sch.IsFullyScheduled);
    }
}
