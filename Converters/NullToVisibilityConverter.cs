using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SchedulerDesktop.Converters;

public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Convert(value, parameter is not "invert");
    }

    private static Visibility Convert(object? obj, bool parameter)
    {
        return obj is null == parameter ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}