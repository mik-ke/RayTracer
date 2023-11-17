using System.Windows.Input;
using RayTracer.Interfaces;
using RayTracer.UI.MVVM;

namespace RayTracer.UI.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IPpmWriter _ppmWriter;
    
    public ICommand AddShapeCommand { get; }
    public ICommand AddLightCommand { get; }
    public ICommand RenderCommand { get; }

    public MainViewModel(IPpmWriter ppmWriter)
    {
        _ppmWriter = ppmWriter;
        
        AddShapeCommand = new RelayCommand(AddShape); 
        AddLightCommand = new RelayCommand(AddLight);
        RenderCommand = new AsyncRelayCommand(RenderAsync);
    }

    private void AddShape(object? obj)
    {
        throw new NotImplementedException();
    }

    private void AddLight(object? obj)
    {
        throw new NotImplementedException();
    }

    private Task RenderAsync(object? arg)
    {
        throw new NotImplementedException();
    }
}