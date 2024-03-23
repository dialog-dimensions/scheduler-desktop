namespace SchedulerDesktop.Utils;

public static class PresentationFuncTools
{
    private static string TimeComponentString(int timeComponent)
    {
        return (timeComponent < 10 ? "0" : string.Empty) + timeComponent;
    }

    public static string DateTimeTimeString(DateTime dateTime)
    {
        return TimeComponentString(dateTime.Hour) + ":" + TimeComponentString(dateTime.Minute);
    }

    public static string DateTimeDateString(DateTime dateTime)
    {
        return dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
    }
}
