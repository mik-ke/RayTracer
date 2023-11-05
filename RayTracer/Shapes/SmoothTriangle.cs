using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// A triangle with smooth shading.
/// </summary>
public class SmoothTriangle : Shape
{
    public Point Point1 { get; }
    public Point Point2 { get; }
    public Point Point3 { get; }
    public Vector Normal1 { get; }
    public Vector Normal2 { get; }
    public Vector Normal3 { get; }


    public SmoothTriangle(Point p1, Point p2, Point p3, Vector n1, Vector n2, Vector n3)
    {
        Point1 = p1;
        Point2 = p2;
        Point3 = p3;
        Normal1 = n1;
        Normal2 = n2;
        Normal3 = n3;
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
