using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;
using SchedulerDesktop.Models.ScheduleEngine;

namespace SchedulerDesktop.Services.Api.Interfaces;

public interface IScheduleApiService : IApiService<Schedule, DateTime, ScheduleDto>
{
    Task<IEnumerable<FlatSchedule>?> GetFlatSchedulesAsync();
    Task<Schedule?> GetScheduleAsync(DateTime key);
    Task<Schedule?> GetLatestScheduleAsync();
    Task<ScheduleResults?> RunSchedulerAsync(DateTime key, Schedule schedule);
    Task<ScheduleData?> GetScheduleDataAsync(DateTime key);
    Task<ScheduleReport?> GetScheduleReportAsync(DateTime key, Schedule schedule);
    Task PostScheduleAsync(Schedule schedule);
    Task AssignEmployeesAsync(Schedule schedule);
    Task<bool> WakeScannerAsync();
    Task<bool> TerminateScannerAsync();
}