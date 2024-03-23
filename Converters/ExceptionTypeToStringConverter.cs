using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using SchedulerDesktop.Enums;

namespace SchedulerDesktop.Converters;

public class ExceptionTypeToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null) return "בחר אילוץ";
        if (value is not ExceptionType exceptionType) throw new NotImplementedException();
        return Convert(exceptionType);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string str) throw new NotImplementedException();
        return ConvertBack(str);
    }

    private string Convert(ExceptionType type)
    {
        return type switch
        {
            ExceptionType.Constraint => "לא יכול להגיע",
            ExceptionType.OffPreference => "מעדיף לא לבצע",
            ExceptionType.OnPreference => "מעדיף לבצע",
            _ => ""
        };
    }

    private ExceptionType ConvertBack(string str)
    {
        return str switch
        {
            "לא יכול להגיע" => ExceptionType.Constraint,
            "מעדיף לא לבצע" => ExceptionType.OffPreference,
            "מעדיף לבצע" => ExceptionType.OnPreference,
            _ => throw new InvalidEnumArgumentException("No such exception type exists.")
        };
    }
}