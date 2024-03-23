using System.Globalization;
using System.Windows.Data;
using SchedulerDesktop.Utils;

namespace SchedulerDesktop.Converters;

public class DateTimeToTimeStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DateTime dateTime) throw new NotImplementedException();
        return Convert(dateTime);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public string Convert(DateTime dateTime)
    {
        return PresentationFuncTools.DateTimeTimeString(dateTime);
    }
}