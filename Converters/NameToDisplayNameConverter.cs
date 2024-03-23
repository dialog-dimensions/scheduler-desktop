using System.Globalization;
using System.Windows.Data;

namespace SchedulerDesktop.Converters;

public class NameToDisplayNameConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string name)
        {
            return Convert(name);
        }
        if (value is null) return "עובד";

        throw new NotImplementedException();
    }

    private string Convert(string name)
    {
        return name == string.Empty ? "עובד" : name;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}