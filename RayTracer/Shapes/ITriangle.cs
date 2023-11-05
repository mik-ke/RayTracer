using RayTracer.Models;

namespace RayTracer.Shapes;

public interface ITriangle
{
    /// <summary>
    /// Intersection that takes into account <paramref name="u"/> and <paramref name="v"/> values.
    /// </summary>
    public Intersection IntersectionWithUV(double t, double u, double v);
}
