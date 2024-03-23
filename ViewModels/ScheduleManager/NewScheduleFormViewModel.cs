using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using SchedulerDesktop.Commands;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Services.Api.Interfaces;
using SchedulerDesktop.ViewModels.BaseClasses;

namespace SchedulerDesktop.ViewModels.ScheduleManager;

public class NewScheduleFormViewModel : ViewModelBase
{
    private readonly IScheduleApiService _apiService;
    private readonly IConfigurationSection _scheduleParams;
    private readonly IConfigurationSection _shiftParams;
    
    public IEnumerable<int> AllowedStartHourValues { get; } = Enumerable.Range(0, 24);
    public IEnumerable<int> AllowedScheduleDurationValues { get; } = Enumerable.Range(1, 31);
    public IEnumerable<int> AllowedShiftDurationValues { get; } = Enumerable.Range(1, 23);

    
    public ICommand ExecuteCommand { get; }

    public NewScheduleFormViewModel(IConfiguration configuration, IScheduleApiService apiService)
    {
        _apiService = apiService;
        _scheduleParams = configuration.GetSection("Params:Schedule");
        _shiftParams = configuration.GetSection("Params:Shift");
        ExecuteCommand = new AsyncRelayCommand(Execute, CanExecute);
        SetDefaultValues();
    }
    
    private void SetDefaultValues()
    {
        StartDateTime = DateTime.Now.Date;
        ScheduleDuration = _scheduleParams.GetValue<int>("Dur:Default");
        ShiftDuration = _shiftParams.GetValue<int>("Dur:Default");
    }

    public async Task SetContinuationToLatestSchedule()
    {
        var latestSchedule = await _apiService.GetLatestScheduleAsync();
        if (latestSchedule is not null)
        {
            StartDateTime = latestSchedule.EndDateTime;
        }
    }
    
    
    private DateTime _startDateTime;
    public DateTime StartDateTime
    {
        get => _startDateTime;
        set
        {
            if (SetField(ref _startDateTime, value))
            {
                StartHour = value.Hour;
                EndDateTime = _startDateTime.AddDays(_scheduleDuration);
            }
        }
    }

    private DateTime _endDateTime;
    public DateTime EndDateTime
    {
        get => _endDateTime;
        set
        {
            var scheduleNewDuration = (int)value.Subtract(StartDateTime).TotalDays;
            var minVal = _scheduleParams.GetValue<int>("Dur:Min");
            var maxVal = _scheduleParams.GetValue<int>("Dur:Max");
            
            if (scheduleNewDuration < minVal)
            {
                EndDateTime = StartDateTime.AddDays(minVal);
                return;
            }
            if (scheduleNewDuration > maxVal)
            {
                EndDateTime = StartDateTime.AddDays(maxVal);
                return;
            }
            if (SetField(ref _endDateTime, value))
            {
                ScheduleDuration = scheduleNewDuration;
            }
        }
    }

    private int _startHour;

    public int StartHour
    {
        get => _startHour;
        set
        {
            if (value < 0 | value > 23) return;
            if (SetField(ref _startHour, value))
            {
                StartDateTime = StartDateTime.Date.AddHours(value);
            }
        }
    }

    private int _scheduleDuration;
    public int ScheduleDuration
    {
        get => _scheduleDuration;
        set
        {
            var minVal = _scheduleParams.GetValue<int>("Dur:Min");
            var maxVal = _scheduleParams.GetValue<int>("Dur:Max");

            if (value < minVal)
            {
                ScheduleDuration = minVal;
                return;
            }
            if (value > maxVal)
            {
                ScheduleDuration = maxVal;
                return;
            }
            if (SetField(ref _scheduleDuration, value))
            {
                EndDateTime = StartDateTime.AddDays(value);
            }
        }
    }

    private int _shiftDuration;

    public int ShiftDuration
    {
        get => _shiftDuration;
        set
        {
            var minVal = _shiftParams.GetValue<int>("Dur:Min");
            var maxVal = _shiftParams.GetValue<int>("Dur:Max");

            if (value < minVal)
            {
                ShiftDuration = minVal;
                return;
            }
            if (value > maxVal)
            {
                ShiftDuration = maxVal;
                return;
            }
            SetField(ref _shiftDuration, value);
        }
    }

    private bool CanExecute()
    {
        return EndDateTime > StartDateTime & 
               ScheduleDuration > 0 & 
               ShiftDuration > 0;
    }

    private async Task Execute()
    {
        await _apiService.PostScheduleAsync(ScheduleFactory.Create(StartDateTime, EndDateTime, ShiftDuration));
        MessageBox.Show("Schedule created successfully.");
    }
}
