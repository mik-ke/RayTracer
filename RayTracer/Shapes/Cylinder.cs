using RayTracer.Extensions;
using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// A cylinder shape. Originates in the object origin with a radius of one (before transformation).
/// </summary>
public class Cylinder : Shape
{
    /// <summary>
    /// The minimum Y component.
    /// </summary>
    public double Minimum { get; init; } = double.NegativeInfinity;
    /// <summary>
    /// The maximum Y component.
    /// </summary>
    public double Maximum { get; init; } = double.PositiveInfinity;
    /// <summary>
    /// Determines if the cylinder is capped at each end.
    /// </summary>
    public bool Closed { get; init; }

    public Cylinder(Matrix? transform = null) : base(transform)
    {
    }

    protected override Intersections LocalIntersect(Ray localRay)
    {
        var a = localRay.Direction.X * localRay.Direction.X +
            localRay.Direction.Z * localRay.Direction.Z;

        List<Intersection> intersections = new();

        if (!IsRayParallel(a))
        {
            IntersectWalls(localRay, intersections, a, out bool hasIntersections);
            if (!hasIntersections) return Intersections.Empty;
        }

        IntersectCaps(localRay, intersections);

        return new Intersections(intersections.ToArray());
    }

    /// <summary>
    /// Checks and adds intersections of cylinder walls.
    /// </summary>
    private void IntersectWalls(Ray localRay, List<Intersection> intersections,
        in double a, out bool hasIntersections)
    {
        var b = 2 * localRay.Origin.X * localRay.Direction.X +
            2 * localRay.Origin.Z * localRay.Direction.Z;
        var c = localRay.Origin.X * localRay.Origin.X +
            localRay.Origin.Z * localRay.Origin.Z - 1;

        var discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            hasIntersections = false;
            return;
        }

        double t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
        double t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

        var y1 = localRay.Origin.Y + t1 * localRay.Direction.Y;
        var y2 = localRay.Origin.Y + t2 * localRay.Direction.Y;

        if (IsYInBounds(y1))
            intersections.Add(new(t1, this));
        if (IsYInBounds(y2))
            intersections.Add(new(t2, this));
        hasIntersections = true;
    }

    /// <summary>
    /// Checks and adds intersections of cylinder caps if <see cref="Closed"/> is true.
    /// </summary>
    private void IntersectCaps(Ray localRay, List<Intersection> intersections)
    {
        if (!Closed || localRay.Direction.Y.IsEqualTo(0))
            return;

        var t = (Minimum - localRay.Origin.Y) / localRay.Direction.Y;
        if (CheckCap(localRay, t))
            intersections.Add(new Intersection(t, this));

        t = (Maximum - localRay.Origin.Y) / localRay.Direction.Y;
        if (CheckCap(localRay, t))
            intersections.Add(new Intersection(t, this));
    }

    /// <summary>
    /// Checks if the intersection at <paramref name="t"/> is within a radius of one
    /// (the radius of cylinders in object space) from the y axis.
    /// </summary>
    private bool CheckCap(Ray localRay, in double t)
    {
        var x = localRay.Origin.X + t * localRay.Direction.X;
        var z = localRay.Origin.Z + t * localRay.Direction.Z;

        return (x * x + z * z) <= 1;
    }

    private static bool IsRayParallel(in double a) => a.IsEqualTo(0);

    private bool IsYInBounds(in double y) => Minimum < y && y < Maximum;

    protected override Vector LocalNormal(Point localPoint)
    {
        double distance = localPoint.X * localPoint.X +
            localPoint.Z * localPoint.Z;

        if (IsPointOnMaximumCap(distance, localPoint))
            return new(0, 1, 0);
        else if (IsPointOnMinimumCap(distance, localPoint))
            return new(0, -1, 0);

        return new(localPoint.X, 0, localPoint.Z);
    }

    private bool IsPointOnMaximumCap(in double distance, Point localPoint) =>
        distance < 1 && localPoint.Y >= Maximum - DoubleExtensions.EPSILON;
    private bool IsPointOnMinimumCap(in double distance, Point localPoint) =>
        distance < 1 && localPoint.Y <= Minimum + DoubleExtensions.EPSILON;
}
