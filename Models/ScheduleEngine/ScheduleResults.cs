using SchedulerDesktop.Models.Entities;

namespace SchedulerDesktop.Models.ScheduleEngine;

public class ScheduleResults
{
    public Schedule CompleteSchedule { get; set; }
    public ScheduleReport Report { get; set; }
}
