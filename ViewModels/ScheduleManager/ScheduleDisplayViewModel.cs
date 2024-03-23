using System.Globalization;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SchedulerDesktop.Commands;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.Models.ScheduleEngine;
using SchedulerDesktop.Services.Api.Interfaces;
using SchedulerDesktop.ViewModels.BaseClasses;

namespace SchedulerDesktop.ViewModels.ScheduleManager;

public class ScheduleDisplayViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IScheduleApiService _scheduleApiService;
    private readonly IShiftApiService _shiftApiService;
    public bool Initialized { get; private set; }
    public Schedule? Schedule { get; private set; }
    public IEnumerable<Employee>? Employees { get; private set; }
    public IEnumerable<ShiftException>? Exceptions { get; private set; }
    
    public List<ShiftCubeViewModel>? Cubes { get; private set; }

    private IEnumerable<IEnumerable<ShiftCubeViewModel>>? _daysPartition = new List<IEnumerable<ShiftCubeViewModel>>();
    public IEnumerable<IEnumerable<ShiftCubeViewModel>>? DaysPartition
    {
        get => _daysPartition;
        private set
        {
            SetField(ref _daysPartition, value);
        }
    }

    public ICommand CommitCommand { get; set; }
    public ICommand CommitAndExportCommand { get; set; }
    public ICommand RevertCommand { get; set; }
    public ICommand RunSchedulerCommand { get; set; }

    public ScheduleDisplayViewModel(IServiceProvider serviceProvider, IScheduleApiService scheduleApiService, IShiftApiService shiftApiService)
    {
        _serviceProvider = serviceProvider;
        _scheduleApiService = scheduleApiService;
        _shiftApiService = shiftApiService;
        CommitCommand = new AsyncRelayCommand(CommitAsync, CanCommit);
        CommitAndExportCommand = new AsyncRelayCommand(CommitAndExportAsync, CanCommitAndExport);
        RevertCommand = new AsyncRelayCommand(RevertAsync, CanRevert);
        RunSchedulerCommand = new AsyncRelayCommand(RunSchedulerAsync, CanRunScheduler);
    }
    
    public async Task InitializeAsync(DateTime scheduleKey)
    {
        var schedule = await _scheduleApiService.GetScheduleDataAsync(scheduleKey);
        if (schedule is null)
        {
            return;
        }

        Initialize(schedule);
    }
    private void Initialize(ScheduleData data)
    {
        Schedule = data.Schedule;
        Employees = data.Employees;
        Exceptions = data.Exceptions;
        CreateCubes();
        Initialized = true;
    }
    private void CreateCubes()
    {
        var cubes = Enumerable
            .Range(0, Schedule!.Count)
            .Select(_ => _serviceProvider.GetRequiredService<ShiftCubeViewModel>())
            .ToList();

        for (var i = 0; i < Schedule.Count; i++)
        {
            cubes[i].Initialize(Schedule[i]);
        }
        
        Cubes = cubes;
        OnCubesCreated();
    }
    private void OnCubesCreated()
    {
        CreateDaysPartition();
    }
    private void CreateDaysPartition()
    {
        DaysPartition = Cubes!.GroupBy(cube => cube.ShiftViewModel.Shift!.DisplayDate);
    }

    private async Task CommitAsync()
    {
        var report = await GetReport(Schedule!);
        if (report is null)
        {
            return;
        }
        
        
        var violationsMessage = report.Violations.Any()
            ? $"Preferences and Constraints Violations were found in the schedule: " +
              $"{string.Join(",", report.Violations.Select(v => $"{v.EmployeeId} @ {v.ShiftKey}"))}."
            : "All Preferences and Constraints were met!";
        var msg = MessageBox.Show($"The schedule got a score of {report.Score}. {violationsMessage} Save Schedule?", "Schedule Report", MessageBoxButton.YesNo, MessageBoxImage.Information);
        if (msg == MessageBoxResult.Yes)
        {
            await _scheduleApiService.AssignEmployeesAsync(Schedule);
            MessageBox.Show("Employees updated successfully.");
        }
    }

    private async Task<ScheduleReport?> GetReport(Schedule schedule)
    {
        var report = await _scheduleApiService.GetScheduleReportAsync(schedule.StartDateTime, schedule);
        if (report is null)
        {
            MessageBox.Show("Unable to retrieve report.");
            return null;
        }

        return report;
    }

    private bool CanCommit()
    {
        return Schedule is not null;
    }

    private async Task CommitAndExportAsync()
    {
        await CommitAsync();
        ExportTools.ScheduleToExcel.ExportScheduleWithDialog(new ScheduleData {Schedule = Schedule!, Employees = Employees!, Exceptions = Exceptions ?? new List<ShiftException>()});
    }

    private bool CanCommitAndExport()
    {
        return CanCommit() && (Schedule?.IsFullyManned ?? false);
    }
    
    private async Task RevertAsync()
    {
        var originalSchedule = await _scheduleApiService.GetScheduleAsync(Schedule!.StartDateTime);
        if (originalSchedule is null)
        {
            MessageBox.Show("Unable to revert.");
            return;
        }

        var orderedCubes = Cubes!
            .OrderBy(c => c.ShiftViewModel.Shift!.StartDateTime)
            .ToList();
        
        var orderedOriginal = originalSchedule
            .OrderBy(s => s.StartDateTime)
            .ToList();
        
        for (var i = 0; i < originalSchedule.Count; i++)
        {
            var originalEmployee = orderedOriginal[i].EmployeeId ?? 0;
            orderedCubes[i].SelectEmployee(originalEmployee);
        }
    }

    private bool CanRevert()
    {
        return Initialized;
    }

    private async Task RunSchedulerAsync()
    {
        var results = await _scheduleApiService.RunSchedulerAsync(Schedule!.StartDateTime, Schedule!);
        if (results is null)
        {
            MessageBox.Show("Results returned null.");
            return;
        }

        MessageBox.Show(results.Report.ToString(), "Report", MessageBoxButton.OK, MessageBoxImage.Information);
        
        ApplyAssignments(results.CompleteSchedule);
        MessageBox.Show(results.Report.Score.ToString(CultureInfo.CurrentCulture), "Success.");
    }

    private void ApplyAssignments(Schedule assignedSchedule)
    {
        var orderedCubes = Cubes!
            .OrderBy(c => c.ShiftViewModel.Shift!.StartDateTime)
            .ToList();
        var orderedAssignedSchedule = assignedSchedule
            .OrderBy(s => s.StartDateTime)
            .ToList();

        for (var i = 0; i < Schedule!.Count; i++)
        {
            orderedCubes[i].SelectEmployee(orderedAssignedSchedule[i].EmployeeId!.Value);
        }
    }

    private bool CanRunScheduler()
    {
        return Initialized && (!Schedule?.IsFullyManned ?? false);
    }
}

