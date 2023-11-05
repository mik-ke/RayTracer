using RayTracer.Extensions;
using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// A triangle shape. Composed of three <see cref="Point"/>s.
/// </summary>
public sealed class Triangle : Shape, ITriangle
{
    public Point Point1 { get; }
    public Point Point2 { get; }
    public Point Point3 { get; }
    public Vector Edge1 { get; }
    public Vector Edge2 { get; }
    /// <summary>
    /// Precomputed normal used for every point of the triangle.
    /// </summary>
    public Vector NormalVector { get; }

    public Triangle(Point point1, Point point2, Point point3, Matrix? transform = null) : base(transform)
    {
        Point1 = point1;
        Point2 = point2;
        Point3 = point3;

        Edge1 = Point2 - Point1;
        Edge2 = Point3 - Point1;
        NormalVector = Edge2.Cross(Edge1).Normalize();
    }

    protected override Intersections LocalIntersect(Ray localRay)
    {
        var directionCrossEdge2 = localRay.Direction.Cross(Edge2);
        var determinant = Edge1.Dot(directionCrossEdge2);

        if (IsRayParallel(determinant)) return Intersections.Empty;

        var f = 1.0 / determinant;
        var point1ToOrigin = localRay.Origin - Point1;
        var u = f * point1ToOrigin.Dot(directionCrossEdge2);
        if (u < 0 || u > 1) return Intersections.Empty;

        var originCrossEdge1 = point1ToOrigin.Cross(Edge1);
        var v = f * localRay.Direction.Dot(originCrossEdge1);
        if (v < 0 || (u + v) > 1) return Intersections.Empty;

        var t = f * Edge2.Dot(originCrossEdge1);
        Intersection intersection = new(t, this);
        return new Intersections(intersection);
    }

    public Intersection IntersectionWithUV(double t, double u, double v)
    {
        throw new NotImplementedException();
    }

    private static bool IsRayParallel(in double determinant) => Math.Abs(determinant) < DoubleExtensions.EPSILON;

    protected override Vector LocalNormal(Point localPoint) => NormalVector;

    public override BoundingBox BoundsOf()
    {
        BoundingBox box = new();
        box.Add(Point1);
        box.Add(Point2);
        box.Add(Point3);
        return box;
    }
}
