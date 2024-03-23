using System.Globalization;
using System.Windows.Data;

namespace SchedulerDesktop.Converters;

public class DayOfWeekToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DayOfWeek dayOfWeek) throw new NotImplementedException();
        return Convert(dayOfWeek);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string str) throw new NotImplementedException();
        return ConvertBack(str);
    }

    public string Convert(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Sunday => "א",
            DayOfWeek.Monday => "ב",
            DayOfWeek.Tuesday => "ג",
            DayOfWeek.Wednesday => "ד",
            DayOfWeek.Thursday => "ה",
            DayOfWeek.Friday => "ו",
            DayOfWeek.Saturday => "ש",
            _ => throw new ArgumentException("non-existing day of week.")
        };
    }

    public DayOfWeek ConvertBack(string str)
    {
        return str switch
        {
            "א" => DayOfWeek.Sunday,
            "ב" => DayOfWeek.Monday,
            "ג" => DayOfWeek.Tuesday,
            "ד" => DayOfWeek.Wednesday,
            "ה" => DayOfWeek.Thursday,
            "ו" => DayOfWeek.Friday,
            "ש" => DayOfWeek.Saturday,
            _ => throw new ArgumentException("non-existing day of week.")
        };
    }
}