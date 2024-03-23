using System.Windows.Input;

namespace SchedulerDesktop.Commands;

public class AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null) : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return canExecute == null || canExecute();
    }

    public async void Execute(object? parameter)
    {
        await execute();
    }
}
