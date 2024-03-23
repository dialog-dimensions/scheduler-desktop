using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SchedulerDesktop.ViewModels.ScheduleManager;

namespace SchedulerDesktop.Converters;

public class CubeBackgroundConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ShiftCubeViewModel vm) throw new NotImplementedException();
        return CubeToBrush(vm);
    }

    private static Color CubeToColor(ShiftCubeViewModel cube)
    {
        if (cube.ShiftViewModel.Shift is null) return Colors.Transparent;
        return cube.ShiftViewModel.Shift.IsDifficult() switch
        {
            true => Colors.MistyRose,
            false => Colors.Transparent
        };
    }

    private static SolidColorBrush ColorToBrush(Color color)
    {
        return new SolidColorBrush(color);
    }

    private static SolidColorBrush CubeToBrush(ShiftCubeViewModel cube) => ColorToBrush(CubeToColor(cube));

    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
