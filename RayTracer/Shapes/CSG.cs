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
        // TODO
        throw new NotImplementedException();
    }

    protected override Intersections LocalIntersect(Ray localRay)
    {
        // TODO
        //if (!BoundsOf().Intersects(localRay))
            //return Intersections.Empty;

        var leftIntersections = Left.Intersect(localRay);
        var rightIntersections = Right.Intersect(localRay);

        var intersections = new Intersections(leftIntersections.Concat(rightIntersections).ToArray());

        return FilterIntersections(intersections);
    }

    public Intersections FilterIntersections(Intersections intersections)
    {
        bool inLeft = false;
        bool inRight = false;

        var result = new List<Intersection>();
        foreach (var intersection in intersections)
        {
            bool leftHit = Left.Includes(intersection.Object);
            if (IntersectionAllowed(Operation, leftHit, inLeft, inRight))
                result.Add(intersection);

            if (leftHit)
                inLeft = !inLeft;
            else
                inRight = !inRight;
        }

        return new Intersections(result.ToArray());
    }

    public override bool Includes(Shape other)
    {
        return Left.Includes(other) || Right.Includes(other);
    }

    protected override Vector LocalNormal(Point localPoint)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Defines whether an intersection is allowed based on the given parameters.
    /// </summary>
    /// <param name="operation">The CSG operation being evaluated.</param>
    /// <param name="leftHit">True if the <see cref="Left"/> was hit and false if <see cref="Right"/> was hit.</param>
    /// <param name="inLeft">True if the hit occurs inside <see cref="Left"/>.</param>
    /// <param name="inRight">True if the hit occursinside <see cref="Right"/>.</param>
    /// <exception cref="NotImplementedException">Thrown if the given <paramref name="operation"/> is not supported.</exception>
    public static bool IntersectionAllowed(Operation operation, bool leftHit, bool inLeft, bool inRight)
    {
        if (operation == Operation.Union)
            return (leftHit && !inRight) || (!leftHit && !inLeft);
        else if (operation == Operation.Intersection)
            return (leftHit && inRight) || (!leftHit && inLeft);
        else if (operation == Operation.Difference)
            return (leftHit && !inRight) || (!leftHit && inLeft);
        else
            throw new NotImplementedException();
    }
}

public enum Operation
{
    Union,
    Intersection,
    Difference
}
