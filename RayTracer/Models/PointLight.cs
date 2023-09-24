namespace RayTracer.Models;

/// <summary>
/// Light source with no size, existing at a single point in space.
/// Defined by its intensity, i.e. how bright it is.
/// </summary>
public class PointLight
{
    public Color Intensity { get; init; }
    public Point Position { get; init; }

    public PointLight(Color intensity, Point position)
    {
        Intensity = intensity;
        Position = position;
    }
}
