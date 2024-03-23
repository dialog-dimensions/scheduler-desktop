using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;

namespace SchedulerDesktop.Services.Api.Interfaces;

public interface IShiftExceptionApiService : IApiService<ShiftException, (DateTime, int) ,ShiftExceptionDto>
{
    Task<IEnumerable<ShiftException>?> GetExceptionsAsync();

    Task<IEnumerable<ShiftException>?> GetEmployeeExceptionsAsync(int employeeId);

    Task<IEnumerable<ShiftException>?> GetScheduleExceptionsAsync(DateTime scheduleKey);

    Task<ShiftException?> GetExceptionAsync((DateTime, int) key);

    Task PostExceptionAsync(ShiftException exception);

    Task DeleteExceptionAsync((DateTime, int) key);
}