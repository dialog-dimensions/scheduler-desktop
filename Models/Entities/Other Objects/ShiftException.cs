using SchedulerDesktop.Enums;
using static SchedulerDesktop.Enums.ExceptionType;

namespace SchedulerDesktop.Models.Entities.Other_Objects;

public class ShiftException(ExceptionType type, DateTime shiftKey, int employeeId, string? reason) : IEquatable<ShiftException>
{
    public ExceptionType ExceptionType { get; } = type;
    public DateTime ShiftKey { get; } = shiftKey;
    public int EmployeeId { get; } = employeeId;
    public string? Reason { get; } = reason;

    public static IEnumerable<string> ColumnNames => new List<string>
    {
        "ExceptionType", "ShiftKey", "EmployeeId", "Reason"
    };


    public static string KeyColumnName => throw new ArgumentException("The model has a composite key");

    public Dictionary<string, object?> DatabaseDictionary =>
        new()
        {
            { "ExceptionType", ExceptionType },
            { "ShiftStartDateTime", ShiftKey },
            { "EmployeeId", EmployeeId },
            { "Reason", Reason }
        };


    public object Key => HashCode.Combine(ShiftKey, EmployeeId);


    public static ShiftException FromDictionary(Dictionary<string, object?> dictionary)
    {
        var exceptionType = (ExceptionType)Convert.ToInt32(dictionary["ExceptionType"]);
        var shiftKey = Convert.ToDateTime(dictionary["ShiftKey"]);
        var employeeId = Convert.ToInt32(dictionary["EmployeeId"]);
        var reason = Convert.ToString(dictionary["Reason"]);

        return exceptionType switch
        {
            OnPreference => ShiftExceptionFactory.CreateOnPreference(shiftKey, employeeId, reason),
            OffPreference => ShiftExceptionFactory.CreateOffPreference(shiftKey, employeeId, reason),
            Constraint => ShiftExceptionFactory.CreateConstraint(shiftKey, employeeId, reason),
            _ => throw new ArgumentException("Undefined exception type.")
        };
    }

    public static ShiftException CreateBlank()
    {
        return ShiftExceptionFactory.CreateBlank();
    }

    public bool Equals(ShiftException? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ExceptionType == other.ExceptionType && 
               ShiftKey.Equals(other.ShiftKey) &&
               EmployeeId == other.EmployeeId && 
               Reason == other.Reason;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ShiftException)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)ExceptionType, ShiftKey, EmployeeId, Reason);
    }
}

public static class ShiftExceptionFactory
{
    public static ShiftException Create(ExceptionType type, DateTime shiftKey, int employeeId, string? reason = "")
    {
        return new ShiftException(type, shiftKey, employeeId, reason);
    }

    public static ShiftException CreateOnPreference(DateTime shiftKey, int employeeId, string? reason = "")
    {
        return Create(OnPreference, shiftKey, employeeId, reason);
    }

    public static ShiftException CreateOffPreference(DateTime shiftKey, int employeeId, string? reason = "")
    {
        return Create(OffPreference, shiftKey, employeeId, reason);
    }

    public static ShiftException CreateConstraint(DateTime shiftKey, int employeeId, string? reason)
    {
        if (reason?.Equals("") ?? true) throw new ArgumentException("Reason is mandatory for constraints.");
        return Create(Constraint, shiftKey, employeeId, reason);
    }

    public static ShiftException CreateBlank()
    {
        return new ShiftException(NoException, DateTime.MinValue, 0, null);
    }
}