using NSubstitute;
using RayTracer.Interfaces;
using RayTracer.UI.ViewModels;

namespace RayTracer.UI.Tests.Unit;

public class MainViewModelTests
{
    private readonly MainViewModel _sut;
    private readonly IPpmWriter _ppmWriter;
    
    public MainViewModelTests()
    {
        _ppmWriter = Substitute.For<IPpmWriter>();
        _sut = new MainViewModel(_ppmWriter);
    }
}