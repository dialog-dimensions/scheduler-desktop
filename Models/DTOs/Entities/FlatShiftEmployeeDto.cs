﻿using SchedulerDesktop.Models.DTOs.Interfaces;
using SchedulerDesktop.Models.Entities;

namespace SchedulerDesktop.Models.DTOs.Entities;

public class FlatShiftEmployeeDto : IDto<Shift, FlatShiftEmployeeDto>
{
    public DateTime ShiftKey { get; set; }
    public Employee? Employee { get; set; }

    public static FlatShiftEmployeeDto FromEntity(Shift entity) => new()
    {
        ShiftKey = entity.StartDateTime,
        Employee = entity.Employee
    };

    public Shift ToEntity()
    {
        throw new NotImplementedException();
    }
}