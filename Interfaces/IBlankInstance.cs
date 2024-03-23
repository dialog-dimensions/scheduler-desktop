namespace SchedulerDesktop.Interfaces;

public interface IBlankInstance<out T>
{
    public static abstract T CreateBlank();
}
