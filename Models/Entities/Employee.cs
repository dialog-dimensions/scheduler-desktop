namespace SchedulerDesktop.Models.Entities;

public class Employee(int id, string name, double balance = 0.0, double difficultBalance = 0.0, bool active = true) : IEquatable<Employee>
{
    public int Id { get; } = id;
    public string Name { get; set; } = name;

    public string Role { get; set; } = "Employee";

    /// <summary>
    ///     The balance of shifts made vs. projected shifts for the employee in all its active schedules.
    ///     Positive balance means the employee has done more shifts than projected.
    /// </summary>
    /// <returns>A double representing the employee's balance.</returns>
    public double Balance { get; set; } = balance;

    public double DifficultBalance { get; set; } = difficultBalance;

    public bool IsActive { get; set; } = active;
    
    public static Employee CreateBlank()
    {
        return EmployeeFactory.CreateBlank();
    }

    public override string ToString()
    {
        return Name;
    }

    public bool Equals(Employee? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Employee)obj);
    }

    public override int GetHashCode()
    {
        return Id;
    }
}

public static class EmployeeFactory
{
    public static Employee Create(int id, string name, double balance, double difficultBalance, bool active = true)
    {
        return new Employee(id, name, balance, difficultBalance, active);
    }

    public static Employee CreateBlank()
    {
        return Create(0, "", 0.0, 0.0);
    }
}