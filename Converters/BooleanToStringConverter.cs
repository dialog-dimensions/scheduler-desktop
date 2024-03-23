using System.Globalization;
using System.Windows.Data;

namespace SchedulerDesktop.Converters;

public class BooleanToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue) return Convert(boolValue);
        throw new NotImplementedException("value is not a boolean.");
    }
    
    private static string Convert(bool value)
    {
        return value ? "V" : string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue) return ConvertBack(stringValue);
        throw new NotImplementedException("value is not a string");
    }

    private static bool ConvertBack(string value) => value switch
    {
        "V" => true,
        "" => false,
        _ => throw new NotImplementedException("string is not convertable to boolean value.")
    };
}
