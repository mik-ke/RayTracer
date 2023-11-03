using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// Constructive solid geometry. Combines two <see cref="Shape"/>s via set operations.
/// </summary>
public sealed class CSG : Shape
{
    public Shape Left { get; init; }
    public Shape Right { get; init; }
    public Operation Operation { get; init; }

    public CSG(Operation operation, Shape left, Shape right, Matrix? transform = null) : base(transform)
    {
        Left = left;
        Right = right;
        Operation = operation;
    }

    public override BoundingBox BoundsOf()
    {
        throw new NotImplementedException();
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

public enum Operation
{
    Union,
    Intersection,
    Difference
}
