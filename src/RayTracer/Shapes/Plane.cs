using RayTracer.Extensions;
using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// A perfectly flat surface that extends infinitely in two dimensions (x and z).
/// </summary>
public sealed class Plane : Shape
{
    public Plane(Matrix? transform = null) : base(transform)
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


    private static readonly Vector _planeNormal = new(0, 1, 0);
    protected override Vector LocalNormal(Point localPoint, Intersection? intersection = null) => _planeNormal;

    public override BoundingBox BoundsOf()
    {
        return new BoundingBox(
            minimum: new(double.NegativeInfinity, 0, double.NegativeInfinity),
            maximum: new(double.PositiveInfinity, 0, double.PositiveInfinity));
    }
}
