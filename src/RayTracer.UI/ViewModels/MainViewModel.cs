using System.Windows.Input;
using RayTracer.Interfaces;
using RayTracer.UI.Models;
using RayTracer.UI.MVVM;

namespace RayTracer.UI.ViewModels;

public class MainViewModel : BaseViewModel
{
    #region Fields
    private readonly IPpmWriter _ppmWriter;
    #endregion
    
    #region Properties
    public List<SceneObject> SceneObjects { get; } = new();
    #endregion
    
    #region Commands
    public ICommand AddSceneObjectCommand { get; }
    public ICommand AddLightCommand { get; }
    public ICommand RenderCommand { get; }
    #endregion

    public MainViewModel(IPpmWriter ppmWriter)
    {
        _ppmWriter = ppmWriter;
        
        AddSceneObjectCommand = new RelayCommand(AddShape); 
        AddLightCommand = new RelayCommand(AddLight);
        RenderCommand = new AsyncRelayCommand(RenderAsync);
    }

    private void AddShape(object? obj)
    {
        SceneObject sceneObject = new()
        {
            ObjectType = SceneObjectType.Sphere
        };
        SceneObjects.Add(sceneObject);
        OnPropertyChanged(nameof(SceneObjects));
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