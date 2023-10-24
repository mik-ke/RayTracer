using RayTracer.Extensions;
using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// A double-napped cone shape.
/// </summary>
public sealed class Cone : Shape
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
    /// Determines if the cone is capped at each end.
    /// </summary>
    public bool Closed { get; init; }

    public Cone(Matrix? transform = null) : base(transform)
    {
    }

    protected override Intersections LocalIntersect(Ray localRay)
    {
        var a = localRay.Direction.X * localRay.Direction.X
            - localRay.Direction.Y * localRay.Direction.Y
            + localRay.Direction.Z * localRay.Direction.Z;

        var b = 2 * localRay.Origin.X * localRay.Direction.X
            - 2 * localRay.Origin.Y * localRay.Direction.Y
            + 2 * localRay.Origin.Z * localRay.Direction.Z;

        var c = localRay.Origin.X * localRay.Origin.X
            - localRay.Origin.Y * localRay.Origin.Y
            + localRay.Origin.Z * localRay.Origin.Z;

        List<Intersection> intersections = new();

        if (IsRayParallel(a))
        {
            if (IsRayParallel(b)) return Intersections.Empty;

            double t = -c / (2 * b);
            intersections.Add(new Intersection(t, this));
        }
        else
        {
            IntersectWalls(localRay, intersections, a, b, c, out bool hasIntersections);
            if (!hasIntersections) return Intersections.Empty;
        }

        IntersectCaps(localRay, intersections);

        return new Intersections(intersections.ToArray());
    }

    /// <summary>
    /// Checks and adds intersections of cone walls.
    /// </summary>
    private void IntersectWalls(Ray localRay, List<Intersection> intersections,
        in double a, in double b, in double c, out bool hasIntersections)
    {
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
    /// Checks and adds intersections of cone caps if <see cref="Closed"/> is true.
    /// </summary>
    private void IntersectCaps(Ray localRay, List<Intersection> intersections)
    {
        if (!Closed || localRay.Direction.Y.IsEqualTo(0))
            return;

        var t = (Minimum - localRay.Origin.Y) / localRay.Direction.Y;
        if (CheckCap(localRay, t, Minimum))
            intersections.Add(new Intersection(t, this));

        t = (Maximum - localRay.Origin.Y) / localRay.Direction.Y;
        if (CheckCap(localRay, t, Maximum))
            intersections.Add(new Intersection(t, this));
    }

    /// <summary>
    /// Checks if the intersection at <paramref name="t"/> is within the radius
    /// (radius = y for napped cones) from the y axis.
    /// </summary>
    private static bool CheckCap(Ray localRay, in double t, in double y)
    {
        var x = localRay.Origin.X + t * localRay.Direction.X;
        var z = localRay.Origin.Z + t * localRay.Direction.Z;

        return (x * x + z * z) <= Math.Abs(y);
    }

    private static bool IsRayParallel(in double i) => i.IsEqualTo(0);

    private bool IsYInBounds(in double y) => Minimum < y && y < Maximum;

    protected override Vector LocalNormal(Point localPoint)
    {
        double distance = localPoint.X * localPoint.X +
            localPoint.Z * localPoint.Z;

        if (IsPointOnMaximumCap(distance, localPoint))
            return new(0, 1, 0);
        else if (IsPointOnMinimumCap(distance, localPoint))
            return new(0, -1, 0);

        double y = Math.Sqrt(localPoint.X * localPoint.X + localPoint.Z * localPoint.Z);
        if (localPoint.Y > 0) y = -y;

        return new(localPoint.X, y, localPoint.Z);
    }

    private bool IsPointOnMaximumCap(in double distance, Point localPoint) =>
        distance < Math.Abs(Maximum) && localPoint.Y >= Maximum - DoubleExtensions.EPSILON;
    private bool IsPointOnMinimumCap(in double distance, Point localPoint) =>
        distance < Math.Abs(Minimum) && localPoint.Y <= Minimum + DoubleExtensions.EPSILON;

    public override BoundingBox BoundsOf()
    {
        var a = Math.Abs(Minimum);
        var b = Math.Abs(Maximum);
        var limit = Math.Max(a, b);

        return new BoundingBox(
            minimum: new(-limit, Minimum, -limit),
            maximum: new(limit, Maximum, limit));
    }
}
