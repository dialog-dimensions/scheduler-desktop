using SchedulerDesktop.Enums;
using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Models.Entities.Other_Objects;

namespace SchedulerDesktop.Models.DTOs.Entities;

public class ShiftSwapDto : IDto<ShiftSwap, ShiftSwapDto>
{
    public int SwapId { get; set; }
    public DateTime ShiftKey { get; set; }
    public int PreviousEmployeeId { get; set; }
    public SwapStatus Status { get; set; }

    public static ShiftSwapDto FromEntity(ShiftSwap entity) => new()
    {
        SwapId = entity.SwapId,
        ShiftKey = entity.ShiftStartDateTime,
        PreviousEmployeeId = entity.PreviousEmployeeId,
        Status = entity.Status
    };

    public ShiftSwap ToEntity() => new(
        SwapId,
        ShiftKey,
        PreviousEmployeeId,
        Status
    );
}