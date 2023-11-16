using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RayTracer.UI.MVVM;

/// <summary>
/// Base class for observable objects.
/// </summary>
public class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    /// <summary>
    /// Sets the value of the specified field and raises the <see cref="PropertyChanged"/> event if the value has changed.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the value has changed, otherwise <see langword="false"/>.
    /// </returns>
    protected bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}