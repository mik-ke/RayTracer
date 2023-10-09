using RayTracer.Extensions;
using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// A cube shape. An axis-aligned bounding box (before transformation).
/// </summary>
public sealed class Cube : Shape
{
    public Cube(Matrix? transform = null) : base(transform) { }

    protected override Vector LocalNormal(Point localPoint)
    {
        var xAbs = Math.Abs(localPoint.X);
        var yAbs = Math.Abs(localPoint.Y);
        var zAbs = Math.Abs(localPoint.Z);

        var maxComponent = Math.Max(xAbs, Math.Max(yAbs, zAbs));

        if (maxComponent == xAbs)
            return new Vector(localPoint.X, 0, 0);
        else if (maxComponent == yAbs)
            return new Vector(0, localPoint.Y, 0);

        return new Vector(0, 0, localPoint.Z);
    }

    protected override Intersections LocalIntersect(Ray localRay)
    {
        var (tMinX, tMaxX) = CheckAxis(localRay.Origin.X, localRay.Direction.X);
        var (tMinY, tMaxY) = CheckAxis(localRay.Origin.Y, localRay.Direction.Y);
        var (tMinZ, tMaxZ) = CheckAxis(localRay.Origin.Z, localRay.Direction.Z);

        var tMin = Math.Max(tMinX, Math.Max(tMinY, tMinZ));
        var tMax = Math.Min(tMaxX, Math.Min(tMaxY, tMaxZ));

        if (tMin > tMax)
            return Intersections.Empty;

        return new Intersections(
            new Intersection(tMin, this),
            new Intersection(tMax, this));
    }

    /// <summary>
    /// Checks where the ray intersects the corresponding plane.
    /// Returns the minimum and maximum t values.
    /// </summary>
    private static (double tMin, double tMax) CheckAxis(double origin, double direction)
    {
        double tMin, tMax;

        var tMinNumerator = -1 - origin;
        var tMaxNumerator = 1 - origin;

        if (Math.Abs(direction) >= DoubleExtensions.EPSILON)
        {
            tMin = tMinNumerator / direction;
            tMax = tMaxNumerator / direction;
        }
        else
        {
            tMin = tMinNumerator * double.PositiveInfinity;
            tMax = tMaxNumerator * double.PositiveInfinity;
        }

        if (tMin > tMax)
            return (tMax, tMin);

        return (tMin, tMax);
    }
}
