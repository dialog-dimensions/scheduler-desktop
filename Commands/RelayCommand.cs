using System.Windows.Input;

namespace SchedulerDesktop.Commands;

public class RelayCommand(Action<object?> execute, Predicate<object?> canExecute) : ICommand
{
    private readonly Action<object?> _execute = execute ?? throw new ArgumentNullException();

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    public bool CanExecute(object? parameter)
    {
        return canExecute(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}