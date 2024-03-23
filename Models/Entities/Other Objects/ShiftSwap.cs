using SchedulerDesktop.Enums;

namespace SchedulerDesktop.Models.Entities.Other_Objects;

public class ShiftSwap(
    int swapId,
    DateTime shiftStartDateTime,
    int previousEmployeeId,
    SwapStatus status = SwapStatus.Applied) : IEquatable<ShiftSwap>
{
    public int SwapId { get; } = swapId;
    public DateTime ShiftStartDateTime { get; } = shiftStartDateTime;
    public int PreviousEmployeeId { get; } = previousEmployeeId;

    public SwapStatus Status { get; set; } = status;

    public static IEnumerable<string> ColumnNames => new List<string>
    {
        "SwapId", "ShiftStartDateTime", "PreviousEmployeeId", "Status"
    };


    public static string KeyColumnName => "SwapId";

    public Dictionary<string, object?> DatabaseDictionary => new()
    {
        { "SwapId", SwapId },
        { "ShiftStartDateTime", ShiftStartDateTime },
        { "PreviousEmployeeId", PreviousEmployeeId },
        { "Status", Status }
    };

    public object Key => SwapId;


    public static ShiftSwap FromDictionary(Dictionary<string, object?> dictionary)
    {
        var swapId = Convert.ToInt32(dictionary["SwapId"]);
        var shiftStartDateTime = Convert.ToDateTime(dictionary["ShiftStartDateTime"]);
        var previousEmployeeId = Convert.ToInt32(dictionary["PreviousEmployeeId"]);
        return ShiftSwapFactory.Create(swapId, shiftStartDateTime, previousEmployeeId);
    }

    public static ShiftSwap CreateBlank()
    {
        return ShiftSwapFactory.CreateBlank();
    }

    public bool Equals(ShiftSwap? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return SwapId == other.SwapId && 
               ShiftStartDateTime.Equals(other.ShiftStartDateTime) && 
               PreviousEmployeeId == other.PreviousEmployeeId &&
               Status == other.Status;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ShiftSwap)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SwapId, ShiftStartDateTime, PreviousEmployeeId, (int)Status);
    }
}

public static class ShiftSwapFactory
{
    public static ShiftSwap Create(int swapId, DateTime shiftStartDateTime, int previousEmployeeId,
        SwapStatus status = SwapStatus.Applied)
    {
        return new ShiftSwap(swapId, shiftStartDateTime, previousEmployeeId, status);
    }

    public static ShiftSwap CreateBlank()
    {
        return Create(0, DateTime.MinValue, 0);
    }
}