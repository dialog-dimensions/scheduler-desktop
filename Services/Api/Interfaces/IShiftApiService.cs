using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.Entities;

namespace SchedulerDesktop.Services.Api.Interfaces;

public interface IShiftApiService : IApiService<Shift, DateTime, ShiftDto>
{
    Task UpdateEmployeeAsync(DateTime key, Shift shift);
    Task UpdateEmployeesAsync(IEnumerable<Shift> shifts);
}