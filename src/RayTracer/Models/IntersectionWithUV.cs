using RayTracer.Shapes;

namespace RayTracer.Models;

/// <summary>
/// <see cref="Intersection"/> with U and V values.
/// Used for representing a location the surface of a triangle, relative to its corners.
/// </summary>
public class IntersectionWithUV : Intersection
{
    public double U { get; init; }
    public double V { get; init; }

    public IntersectionWithUV(double t, Shape @object, double u, double v) : base(t, @object)
    {
        U = u;
        V = v;
    }
}
