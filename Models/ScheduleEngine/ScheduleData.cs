﻿using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;

namespace SchedulerDesktop.Models.ScheduleEngine;

public class ScheduleData
{
    public Schedule Schedule { get; set; }
    public IEnumerable<Employee> Employees { get; set; }
    public IEnumerable<ShiftException> Exceptions { get; set; }

    
    public Shift? FindShift(DateTime shiftKey) => 
        Schedule.FirstOrDefault(shift => shift.StartDateTime == shiftKey);
    
    public Employee? FindEmployee(int employeeId) => 
        Employees.FirstOrDefault(emp => emp.Id == employeeId);

    public ShiftException? FindException(DateTime shiftKey, int employeeId) =>
        Exceptions.FirstOrDefault(ex => ex.EmployeeId == employeeId & ex.ShiftKey == shiftKey);
}