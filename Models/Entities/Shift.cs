namespace SchedulerDesktop.Models.Entities;

public class Shift : IEquatable<Shift>
{
    public const int DefaultShiftDuration = 12;

    private int? _employeeId;
    public DateTime ScheduleStartDateTime { get; init; }
    public DateTime StartDateTime { get; init; }
    public DateTime EndDateTime { get; init; }

    private Employee? _employee;
    public Employee? Employee
    {
        get => _employee;
        set
        {
            _employee = value;
            EmployeeId = value?.Id;
        }
    }
    
    public int? EmployeeId
    {
        get => _employeeId;
        private set
        {
            if (value.Equals(_employeeId)) return;
            switch (value)
            {
                case 0 when _employeeId is null:
                    return;
                case 0:
                    _employeeId = null;
                    break;
                default:
                    _employeeId = value;
                    break;
            }
        }
    }

    public bool IsManned => EmployeeId is not null;
    public DateTime DisplayDate => StartDateTime.Date.AddDays(StartDateTime.Hour < 5 ? -1 : 0);

    private bool IsWeekend()
    {
        return StartDateTime.DayOfWeek is DayOfWeek.Friday or DayOfWeek.Saturday;
    }
    private static bool IsWeekend(DateTime startDateTime)
    {
        return startDateTime.DayOfWeek is DayOfWeek.Friday or DayOfWeek.Saturday;
    }
    public bool IsDifficult()
    {
        return IsWeekend();
    }

    public override string ToString()
    {
        return $"{ScheduleStartDateTime.ToShortDateString()} -> " +
               $"{StartDateTime.ToShortDateString()} {StartDateTime.Hour}" +
               $"{(IsManned ? ':' + EmployeeId : string.Empty)}";
    }

    public bool Equals(Shift? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _employeeId == other._employeeId && 
               ScheduleStartDateTime.Equals(other.ScheduleStartDateTime) && 
               StartDateTime.Equals(other.StartDateTime) && 
               EndDateTime.Equals(other.EndDateTime);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Shift)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_employeeId, ScheduleStartDateTime, StartDateTime, EndDateTime);
    }
}

public static class ShiftFactory
{
    public static Shift Create(DateTime scheduleStartDateTime, DateTime startDateTime, DateTime endDateTime)
    {
        return new Shift
        {
            ScheduleStartDateTime = scheduleStartDateTime,
            StartDateTime = startDateTime,
            EndDateTime = endDateTime
        };
    }

    public static Shift Create(DateTime scheduleStartDateTime, DateTime startDateTime, int duration)
    {
        return Create(
            scheduleStartDateTime,
            startDateTime,
            startDateTime.AddHours(duration)
        );
    }
}