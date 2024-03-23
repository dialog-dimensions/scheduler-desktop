using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SchedulerDesktop.Enums;

namespace SchedulerDesktop.Converters;

public class ExceptionTypeToBackgroundConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ExceptionType type) return Convert(type);
        throw new NotImplementedException("Convert value is not an exception type.");
    }

    private static Color TypeToColor(ExceptionType? type) => type switch
    {
        ExceptionType.Constraint => Colors.PaleVioletRed,
        ExceptionType.OffPreference => Colors.LightPink,
        ExceptionType.OnPreference => Colors.PaleGreen,
        ExceptionType.NoException => Colors.Transparent,
        _ => Colors.Transparent
    };

    private static SolidColorBrush ColorToBrush(Color color) => new(color);

    private static SolidColorBrush Convert(ExceptionType type) => ColorToBrush(TypeToColor(type));

    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}