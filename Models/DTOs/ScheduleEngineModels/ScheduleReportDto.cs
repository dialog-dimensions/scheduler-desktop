﻿using SchedulerDesktop.Models.DTOs.Entities;
using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Models.ScheduleEngine;

namespace SchedulerDesktop.Models.DTOs.ScheduleEngineModels;

public class ScheduleReportDto : IDto<ScheduleReport, ScheduleReportDto>
{
    public ScheduleQuotasDto Quotas { get; set; }
    public IEnumerable<ShiftExceptionDto> Violations { get; set; }
    public IEnumerable<EmployeeIncrementsDto> Increments { get; set; }

    public static ScheduleReportDto FromEntity(ScheduleReport entity) => new()
    {
        Quotas = entity.Quotas,
        Violations = entity.Violations,
        Increments = entity.Increments.Select(EmployeeIncrementsDto.FromEntity)
    };

    public ScheduleReport ToEntity() => new()
    {
        Quotas = Quotas,
        Violations = Violations,
        Increments = Increments.Select(inc => inc.ToEntity())
    };
}
