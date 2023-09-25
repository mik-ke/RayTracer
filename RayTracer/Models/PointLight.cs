namespace RayTracer.Models;

/// <summary>
/// Light source with no size, existing at a single point in space.
/// Defined by its intensity, i.e. how bright it is.
/// </summary>
public sealed class PointLight
{
    public Point Position { get; init; }
    public Color Intensity { get; init; }

    public PointLight(Point position, Color intensity)
    {
        Position = position;
        Intensity = intensity;
    }
}
