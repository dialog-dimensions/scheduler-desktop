using SchedulerDesktop.Models.DTOs.Interfaces;

namespace SchedulerDesktop.Models.DTOs.Entities;

public class EmployeeQuotasDto : IDto<EmployeeQuotasDto, EmployeeQuotasDto>
{
    public int EmployeeId { get; set; }
    public DateTime ScheduleKey { get; set; }
    public double RegularQuota { get; set; }
    public double DifficultQuota { get; set; }

    public static EmployeeQuotasDto FromEntity(EmployeeQuotasDto entity)
    {
        return entity;
    }

    public EmployeeQuotasDto ToEntity()
    {
        return this;
    }
}
