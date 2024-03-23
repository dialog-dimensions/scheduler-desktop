using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.DTOs.ScheduleEngineModels;

namespace SchedulerDesktop.Models.ScheduleEngine;

public class ScheduleReport
{
    public ScheduleQuotasDto Quotas { get; set; }
    public IEnumerable<ShiftExceptionDto> Violations { get; set; }
    public IEnumerable<EmployeeIncrements> Increments { get; set; }
    public double Score { get; set; }

    public override string ToString()
    {
        return $"Score: {Score}\n" + 
               $"Quotas:\n" +
               $"{Quotas.Select(q => $"{q.EmployeeId}: {q.RegularQuota} - {q.DifficultQuota}\n").ToList()}" +
               $"Increments:\n" +
               $"{Increments.Select(i => $"{i.EmployeeId}: {i.RegularIncrement} - {i.DifficultIncrement}\n").ToList()}" +
               $"Violations:\n" +
               $"{Violations.Select(v => $"{v.EmployeeId} - {v.ShiftKey} ({v.ExceptionType.ToString()})").ToList()}";
    }
}