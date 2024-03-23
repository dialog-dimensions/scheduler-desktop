using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SchedulerDesktop.Enums;

namespace SchedulerDesktop.Converters;

public class TemporalStateToForegroundConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TemporalState state) return Convert(state);
        throw new NotImplementedException("Convert value is not a temporal state.");
    }

    private static Color StateToColor(TemporalState? state) => state switch
    {
        TemporalState.Past => Colors.Gray,
        TemporalState.Present => Colors.DimGray,
        TemporalState.Future => Colors.Black,
        _ => Colors.Black
    };

    private static SolidColorBrush ColorToBrush(Color color) => new(color);

    private static SolidColorBrush Convert(TemporalState state) => ColorToBrush(StateToColor(state));

    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}