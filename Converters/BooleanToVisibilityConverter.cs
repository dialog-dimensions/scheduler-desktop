using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SchedulerDesktop.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool boolValue) throw new NotImplementedException();
        return parameter is "invert" ? ConvertInverted(boolValue) : ConvertRegular(boolValue);
    }

    private Visibility ConvertRegular(bool value)
    {
        return value ? Visibility.Visible : Visibility.Collapsed;
    }

    private Visibility ConvertInverted(bool value) => ConvertRegular(!value);

    
    
    
    
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}