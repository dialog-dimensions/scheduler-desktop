using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Models.ScheduleEngine;

namespace SchedulerDesktop.Models.DTOs.ScheduleEngineModels;

public class ScheduleQuotasDto : List<EmployeeQuotasDto>, IDto<ScheduleQuotas, ScheduleQuotasDto>
{
    public DateTime ScheduleKey { get; set; }

    public static ScheduleQuotasDto FromEntity(ScheduleQuotas entity)
    {
        var result = new ScheduleQuotasDto { ScheduleKey = entity.Schedule.StartDateTime };
        result.AddRange(entity.Select(EmployeeQuotasDto.FromEntity));
        return result;
    }

    public ScheduleQuotas ToEntity()
    {
        throw new NotImplementedException();
    }
}