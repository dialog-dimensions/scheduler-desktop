using SchedulerDesktop.Enums;
using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Models.Entities.Other_Objects;

namespace SchedulerDesktop.Models.DTOs.Entities;

public class ShiftExceptionDto : IDto<ShiftException, ShiftExceptionDto>
{
    public DateTime ShiftKey { get; set; }
    public int EmployeeId { get; set; }
    public ExceptionType ExceptionType { get; set; }
    public string? Reason { get; set; }

    public static ShiftExceptionDto FromEntity(ShiftException entity) => new()
    {
        ShiftKey = entity.ShiftKey,
        EmployeeId = entity.EmployeeId,
        ExceptionType = entity.ExceptionType,
        Reason = entity.Reason
    };

    public ShiftException ToEntity() => new(ExceptionType, ShiftKey, EmployeeId, Reason);
}