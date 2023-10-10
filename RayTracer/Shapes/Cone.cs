using RayTracer.Models;

namespace RayTracer.Shapes;

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

        if (a == 0)
        {
            if (b == 0) return Intersections.Empty;
            double t = -c / (2 * b);
            return new Intersections(new Intersection(t, this));
        }

        var discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
            return Intersections.Empty;

        double t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
        double t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

        var y1 = localRay.Origin.Y + t1 * localRay.Direction.Y;
        var y2 = localRay.Origin.Y + t2 * localRay.Direction.Y;

        List<Intersection> intersections = new();
        if (IsYInBounds(y1))
            intersections.Add(new(t1, this));
        if (IsYInBounds(y2))
            intersections.Add(new(t2, this));

        return new Intersections(intersections.ToArray());
    }
    private bool IsYInBounds(in double y) => Minimum < y && y < Maximum;

    protected override Vector LocalNormal(Point localPoint)
    {
        throw new NotImplementedException();
    }
}
