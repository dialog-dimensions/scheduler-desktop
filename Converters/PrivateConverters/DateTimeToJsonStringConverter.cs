namespace SchedulerDesktop.Converters.PrivateConverters;

public class DateTimeToJsonStringConverter
{
    public static string Convert(DateTime dateTime)
    {
        var z = PlantLeadingZeros; 
        return $"{dateTime.Year}-{z(dateTime.Month)}-{z(dateTime.Day)}T{z(dateTime.Hour)}:{z(dateTime.Minute)}:{z(dateTime.Second)}";
    }

    public static string PlantLeadingZeros(int number) => $"{(number < 10 ? "0" : string.Empty)}{number}";
}