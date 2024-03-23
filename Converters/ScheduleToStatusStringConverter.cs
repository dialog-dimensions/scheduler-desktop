using System.Globalization;
using System.Windows.Data;
using SchedulerDesktop.Models.Entities;

namespace SchedulerDesktop.Converters;

public class ScheduleToStatusStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Schedule schedule) throw new NotImplementedException();
        return Convert(schedule);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static string Convert(Schedule schedule)
    {
        return schedule.Count(shift => shift.EmployeeId > 0) + "/" + schedule.Count;
    }
}