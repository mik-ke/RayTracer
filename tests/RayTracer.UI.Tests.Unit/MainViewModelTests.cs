using NSubstitute;
using RayTracer.Interfaces;
using RayTracer.UI.Models;
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
    
    [Fact]
    public void AddSceneObjectCommand_ShouldAddSceneObject_WhenExecuted()
    {
        // Arrange
        const int expectedCount = 1;
        const SceneObjectType expectedType = SceneObjectType.Sphere;
        
        // Act
        _sut.AddSceneObjectCommand.Execute(null);
        
        // Assert
        Assert.Equal(expectedCount, _sut.SceneObjects.Count);
        Assert.Equal(expectedType, _sut.SceneObjects[0].ObjectType);
    }
}