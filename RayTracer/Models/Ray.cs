namespace RayTracer.Models;

public sealed class Ray
{
    public Point Origin { get; init; }
    public Vector Direction { get; init; }

    public Ray(Point origin, Vector direction)
    {
        Origin = origin;
        Direction = direction;
    }

    /// <summary>
    /// Returns the position of the ray after "time"/distance <paramref name="t"/>
    /// </summary>
    /// <param name="t"></param>
    public Point Position(double t)
    {
        return Origin + Direction * t;
    }
}
