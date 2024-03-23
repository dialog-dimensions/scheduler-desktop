namespace SchedulerDesktop.Models.Entities.Other_Objects;

public class FlatSchedule
{
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public int ShiftDuration { get; set; }
    public bool IsFullyScheduled { get; set; }
}