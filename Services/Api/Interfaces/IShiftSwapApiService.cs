using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;

namespace SchedulerDesktop.Services.Api.Interfaces;

public interface IShiftSwapApiService : IApiService<ShiftSwap, int, ShiftSwapDto>
{
    Task<IEnumerable<ShiftSwap>?> GetSwapsAsync();
}