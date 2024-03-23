namespace SchedulerDesktop.Models.Entities;

public class Schedule(DateTime startDateTime, DateTime endDateTime, int shiftDuration) : List<Shift>, IEquatable<Schedule>
{
    public const int DefaultScheduleDuration = 7;
    public DateTime StartDateTime { get; } = startDateTime;
    public DateTime EndDateTime { get; } = endDateTime;
    public int ShiftDuration { get; } = shiftDuration;

    public int CountShifts => Count;

    public static IEnumerable<string> ColumnNames => new List<string>
        { "StartDateTime", "EndDateTime", "ShiftDuration" };


    public static string KeyColumnName => "StartDateTime";

    public Dictionary<string, object?> DatabaseDictionary => new()
    {
        { "StartDateTime", StartDateTime },
        { "EndDateTime", EndDateTime },
        { "ShiftDuration", ShiftDuration }
    };


    public object Key => StartDateTime;

    public bool IsFullyManned => this.All(shift => shift.IsManned);

    public IEnumerable<Shift> DifficultShifts =>
        this.Where(shift => shift.IsDifficult()).OrderBy(shift => shift.StartDateTime);

    public static Schedule FromDictionary(Dictionary<string, object?> dictionary)
    {
        var startDateTime = Convert.ToDateTime(dictionary["StartDateTime"]);
        var endDateTime = Convert.ToDateTime(dictionary["EndDateTime"]);
        var shiftDuration = Convert.ToInt32(dictionary["ShiftDuration"]);
        return ScheduleFactory.CreateEmpty(startDateTime, endDateTime, shiftDuration);
    }

    public Shift? Find(object shiftKey)
    {
        return Find(shift => shiftKey.Equals(shift.StartDateTime));
    }

    public static Schedule CreateBlank()
    {
        return ScheduleFactory.CreateBlank();
    }

    public override string ToString()
    {
        return StartDateTime.ToShortDateString() + (IsFullyManned ? string.Empty : "*");
    }

    public bool Equals(Schedule? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return StartDateTime.Equals(other.StartDateTime) && 
               EndDateTime.Equals(other.EndDateTime) && 
               ShiftDuration == other.ShiftDuration;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Schedule)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartDateTime, EndDateTime, ShiftDuration);
    }
}

public static class ScheduleFactory
{
    public static Schedule CreateEmpty(DateTime startDateTime, DateTime endDateTime,
        int shiftDuration = Shift.DefaultShiftDuration)
    {
        return new Schedule(startDateTime, endDateTime, shiftDuration);
    }

    public static Schedule Create(DateTime startDateTime, DateTime endDateTime,
        int shiftDuration = Shift.DefaultShiftDuration)
    {
        var schedule = CreateEmpty(startDateTime, endDateTime, shiftDuration);
        InitializeShifts(schedule);
        return schedule;
    }

    public static Schedule Create(DateTime startDateTime,
        int scheduleDuration = Schedule.DefaultScheduleDuration, int shiftDuration = Shift.DefaultShiftDuration)
    {
        return Create(
            startDateTime,
            startDateTime.AddDays(scheduleDuration),
            shiftDuration
        );
    }

    private static void InitializeShifts(Schedule schedule)
    {
        var shiftStartDateTime = schedule.StartDateTime;
        while (shiftStartDateTime < schedule.EndDateTime)
        {
            schedule.Add(
                ShiftFactory.Create(schedule.StartDateTime, shiftStartDateTime, schedule.ShiftDuration)
            );
            shiftStartDateTime = shiftStartDateTime.AddHours(schedule.ShiftDuration);
        }
    }

    public static Schedule FromShifts(IEnumerable<Shift> shifts)
    {
        var orderedShifts = shifts.OrderBy(shift => shift.StartDateTime).ToList();

        var startDateTime = orderedShifts[0].StartDateTime;
        var endDateTime = orderedShifts[^1].EndDateTime;
        var shiftDuration = (int)orderedShifts[0].EndDateTime.Subtract(orderedShifts[0].StartDateTime).TotalHours;

        var result = CreateEmpty(startDateTime, endDateTime, shiftDuration);
        result.AddRange(orderedShifts);
        return result;
    }

    public static Schedule CreateClearDuplicate(Schedule schedule)
    {
        return Create(schedule.StartDateTime, schedule.EndDateTime, schedule.ShiftDuration);
    }

    public static Schedule CreateBlank()
    {
        return CreateEmpty(DateTime.MinValue, DateTime.MinValue, 0);
    }
}