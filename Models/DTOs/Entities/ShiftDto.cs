using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Models.Entities;

namespace SchedulerDesktop.Models.DTOs.Entities;

public class ShiftDto : IDto<Shift, ShiftDto>
{
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public DateTime ScheduleKey { get; set; }
    public EmployeeDto? Employee { get; set; }

    public static ShiftDto FromEntity(Shift entity) => new()
    {
        StartDateTime = entity.StartDateTime,
        EndDateTime = entity.EndDateTime,
        ScheduleKey = entity.ScheduleStartDateTime,
        Employee = entity.Employee is null ? null : EmployeeDto.FromEntity(entity.Employee),
    };

    public Shift ToEntity()
    {
        throw new NotImplementedException();
    }
}
