using System.Windows.Media;
using SchedulerDesktop.Enums;
using SchedulerDesktop.Models.Entities;
using SchedulerDesktop.Models.Entities.Other_Objects;

namespace SchedulerDesktop.Models.Items;

public class EmployeeItem : IEquatable<EmployeeItem>
{
    public Employee Employee { get; set; }
    public ExceptionType ExceptionType { get; set; }

    public EmployeeItem(Employee employee, ShiftException? shiftException = default)
    {
        Employee = employee;
        ExceptionType = shiftException?.ExceptionType ?? ExceptionType.NoException;
    }

    public SolidColorBrush ForegroundColorBrush => ExceptionType switch
    {
        ExceptionType.OnPreference => new SolidColorBrush(Colors.PaleGreen),
        ExceptionType.OffPreference => new SolidColorBrush(Colors.PaleVioletRed),
        ExceptionType.Constraint => new SolidColorBrush(Colors.Red),
        _ => new SolidColorBrush(Colors.Transparent)
    };

    public bool Equals(EmployeeItem? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Employee.Equals(other.Employee) && ExceptionType == other.ExceptionType;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((EmployeeItem)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Employee, (int)ExceptionType);
    }
}
