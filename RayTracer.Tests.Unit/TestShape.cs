using RayTracer.Shapes;
using RayTracer.Models;

namespace RayTracer.Tests.Unit;

/// <summary>
/// Used solely for testing abstract functionality of <see cref="Shape"/>.
/// </summary>
public sealed class TestShape : Shape
{
    public TestShape() : this(null)
    {
    }
    public TestShape(Matrix? transform) : base(transform)
    {
    }

    public Ray? SavedLocalRay;
    protected override Intersections LocalIntersect(Ray localRay)
    {
        SavedLocalRay = localRay;
        return null!;
    }

    protected override Vector LocalNormal(Point localPoint) =>
        new(localPoint.X, localPoint.Y, localPoint.Z);

    public override BoundingBox BoundsOf()
    {
        return new BoundingBox(new(-1, -1, -1), new(1, 1, 1));
    }
}
