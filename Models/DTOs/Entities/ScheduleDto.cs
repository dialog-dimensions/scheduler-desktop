using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Models.Entities;

namespace SchedulerDesktop.Models.DTOs.Entities;

public class ScheduleDto : List<ShiftDto>, IDto<Schedule, ScheduleDto>
{
    public DateTime? StartDateTime => this.MinBy(s => s.StartDateTime)?.StartDateTime;
    public DateTime? EndDateTime => this.MaxBy(s => s.EndDateTime)?.EndDateTime;

    public int? ShiftDuration => StartDateTime.HasValue & EndDateTime.HasValue
        ? (int)double.Round(EndDateTime!.Value.Subtract(StartDateTime!.Value).TotalHours / Count)
        : null;
    
    public static ScheduleDto FromEntity(Schedule entity)
    {
        var result = new ScheduleDto();
        result.AddRange(entity.Select(ShiftDto.FromEntity));
        return result;
    }
    
    public Schedule ToEntity()
    {
        var result = new Schedule(StartDateTime!.Value, EndDateTime!.Value, ShiftDuration!.Value);
        result.AddRange(this.Select(shiftDto => new Shift
        {
            StartDateTime = shiftDto.StartDateTime,
            EndDateTime = shiftDto.EndDateTime,
            ScheduleStartDateTime = StartDateTime!.Value,
            Employee = shiftDto.Employee?.ToEntity()
        }));

        return result;
    }
}
