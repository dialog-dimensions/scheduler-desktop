using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Models.Entities.Other_Objects;

namespace SchedulerDesktop.Models.DTOs.Entities;

public class FlatScheduleDto : FlatSchedule, IDto<FlatSchedule, FlatScheduleDto>
{
    public static FlatScheduleDto FromEntity(FlatSchedule entity) => new()
    {
        StartDateTime = entity.StartDateTime,
        EndDateTime = entity.EndDateTime,
        ShiftDuration = entity.ShiftDuration,
        IsFullyScheduled = entity.IsFullyScheduled
    };

    public FlatSchedule ToEntity() => this;
}
