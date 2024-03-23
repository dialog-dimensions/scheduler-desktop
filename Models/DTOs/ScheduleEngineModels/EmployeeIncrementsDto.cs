using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Models.ScheduleEngine;

namespace SchedulerDesktop.Models.DTOs.ScheduleEngineModels;

public class EmployeeIncrementsDto : EmployeeIncrements, IDto<EmployeeIncrements, EmployeeIncrementsDto>
{
    public static EmployeeIncrementsDto FromEntity(EmployeeIncrements entity) => new()
    {
        EmployeeId = entity.EmployeeId,
        RegularIncrement = entity.RegularIncrement, 
        DifficultIncrement = entity.DifficultIncrement
    };

    public EmployeeIncrements ToEntity()
    {
        return this;
    }
}
