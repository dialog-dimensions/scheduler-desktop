using System.Globalization;
using System.Windows.Data;

namespace SchedulerDesktop.Converters;

public class NullToBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Convert(value) == parameter is not "invert";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private bool Convert(object? value)
    {
        return value is not null;
    }
}