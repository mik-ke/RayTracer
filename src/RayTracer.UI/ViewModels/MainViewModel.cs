using RayTracer.Interfaces;

namespace RayTracer.UI.ViewModels;

public class MainViewModel
{
    private readonly IPpmWriter _ppmWriter;

    public MainViewModel(IPpmWriter ppmWriter)
    {
        _ppmWriter = ppmWriter;
    }
}