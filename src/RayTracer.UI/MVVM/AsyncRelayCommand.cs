using System.Windows.Input;

namespace RayTracer.UI.MVVM;

public class AsyncRelayCommand : ICommand
{
    private readonly Func<object?, Task> _executeAsync;
    private readonly Func<object?, bool>? _canExecute;
    
    public event EventHandler? CanExecuteChanged;

    public AsyncRelayCommand(Func<object?, Task> executeAsync, Func<object?, bool>? canExecute = null)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) =>
        _canExecute == null || _canExecute(parameter);

    public async void Execute(object? parameter)
    {
        await ExecuteAsync(parameter);
    }
    
    public async Task ExecuteAsync(object? parameter)
    {
        await _executeAsync(parameter);
        RaiseCanExecuteChanged();
    }

    public void RaiseCanExecuteChanged() =>
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}