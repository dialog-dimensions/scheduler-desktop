using System.Globalization;
using System.Windows.Data;
using SchedulerDesktop.ViewModels.ScheduleManager;

namespace SchedulerDesktop.Converters;

public class ShiftCubeViewModelToTimeRangeStringConverter : IValueConverter
{
    private readonly DateTimeToTimeStringConverter _timeStringConverter = new();
    
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ShiftCubeViewModel cube) throw new ArgumentException("");
        return Convert(cube);
    }

    
    private string Convert(ShiftCubeViewModel cube)
    {
        return _timeStringConverter.Convert(cube.ShiftViewModel.Shift!.StartDateTime) + " - " +
               _timeStringConverter.Convert(cube.ShiftViewModel.Shift.EndDateTime);
    }
    
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}