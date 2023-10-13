using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// A container for multiple <see cref="Shape"/>s.
/// </summary>
public sealed class Group : Shape
{
    private List<Shape> _shapes;

    public Group(Matrix? transform = null) : base(transform)
    {
        _shapes = new();
    }

    protected override Intersections LocalIntersect(Ray localRay)
    {
        throw new NotImplementedException();
    }

    protected override Vector LocalNormal(Point localPoint)
    {
        throw new NotImplementedException();
    }
}
