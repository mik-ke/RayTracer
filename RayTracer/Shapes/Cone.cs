using RayTracer.Models;

namespace RayTracer.Shapes;

public class Cone : Shape
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
            if (b == 0)
                return Intersections.Empty;
        }

        return null!;
    }

    protected override Vector LocalNormal(Point localPoint)
    {
        throw new NotImplementedException();
    }
}
