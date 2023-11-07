using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// A triangle with smooth shading.
/// </summary>
public sealed class SmoothTriangle : Triangle
{
    public Vector Normal1 { get; }
    public Vector Normal2 { get; }
    public Vector Normal3 { get; }


    public SmoothTriangle(Point point1, Point point2, Point point3,
        Vector normal1, Vector normal2, Vector normal3) : base(point1, point2, point3)
    {
        Normal1 = normal1;
        Normal2 = normal2;
        Normal3 = normal3;
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
        IntersectionWithUV intersection = new(t, this, u, v);
        return new Intersections(intersection);
    }

    protected override Vector LocalNormal(Point localPoint)
    {
        throw new NotImplementedException();
    }
}
