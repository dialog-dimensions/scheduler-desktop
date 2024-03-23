using System.Globalization;
using System.Windows.Data;
using SchedulerDesktop.Utils;

namespace SchedulerDesktop.Converters;

public class DateTimeToDateStringConverter : IValueConverter
{
    private readonly DayOfWeekToStringConverter _dayOfWeekToStringConverter = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DateTime dateTime) throw new NotImplementedException();
        return Convert(dateTime);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private string Convert(DateTime dateTime)
    {
        return _dayOfWeekToStringConverter.Convert(dateTime.DayOfWeek) + " " + PresentationFuncTools.DateTimeDateString(dateTime);
    }
}