using RayTracer.Extensions;
using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// A perfectly flat surface that extends infinitely in two dimensions (x and z).
/// </summary>
public class Plane : Shape
{
    public Plane() : this(null) { }
    public Plane(Matrix? transform) : base(transform)
    {
    }

    protected override Intersections LocalIntersect(Ray localRay)
    {
        if (IsRayParallel(localRay))
            return Intersections.Empty;

        double t = -localRay.Origin.Y / localRay.Direction.Y;
        Intersection intersection = new(t, this);
        return new Intersections(intersection);
    }
    private static bool IsRayParallel(Ray ray) =>
        Math.Abs(ray.Direction.Y) < DoubleExtensions.EPSILON;


    private static Vector _planeNormal = new Vector(0, 1, 0);
    protected override Vector LocalNormal(Point localPoint) => _planeNormal;
}
