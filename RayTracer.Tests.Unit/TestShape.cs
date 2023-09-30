using RayTracer.Extensions.Shapes;
using RayTracer.Models;

namespace RayTracer.Tests.Unit;

/// <summary>
/// Used solely for testing abstract functionality of <see cref="Shape"/>.
/// </summary>
public sealed class TestShape : Shape
{
    public TestShape()
    {
    }
    public TestShape(Matrix transform) : base(transform)
    {
    }

    public Ray? SavedLocalRay;
    protected override Intersections LocalIntersect(Ray localRay)
    {
        SavedLocalRay = localRay;
        return null!;
    }

    protected override Vector LocalNormal(Point localPoint) =>
        new Vector(localPoint.X, localPoint.Y, localPoint.Z);
}
