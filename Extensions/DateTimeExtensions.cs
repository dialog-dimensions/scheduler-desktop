using SchedulerDesktop.Converters.PrivateConverters;

namespace SchedulerDesktop.Extensions;

public static class DateTimeExtensions
{
    public static string ToJsonString(this DateTime dateTime)
    {
        return DateTimeToJsonStringConverter.Convert(dateTime);
    }
}
