using RayTracer.UI.MVVM;

namespace RayTracer.UI.Models;

/// <summary>
/// Represents a scene object.
/// </summary>
public class SceneObject : ObservableObject
{
    #region fields
    private SceneObjectType _objectType;
    #endregion
    
    #region properties
    public SceneObjectType ObjectType
    {
        get => _objectType;
        set => Set(ref _objectType, value);
    }
    #endregion
}

public enum SceneObjectType
{
    Cone,
    CSG,
    Cube,
    Cylinder,
    Group,
    Plane,
    SmoothTriangle,
    Sphere,
    Triangle
}