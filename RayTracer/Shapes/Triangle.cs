﻿using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// A triangle shape. Composed of three <see cref="Point"/>s.
/// </summary>
public class Triangle : Shape
{
    public Point Point1 { get; }
    public Point Point2 { get; }
    public Point Point3 { get; }
    public Vector Edge1 { get; }
    public Vector Edge2 { get; }
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
        throw new NotImplementedException();
    }

    protected override Vector LocalNormal(Point localPoint)
    {
        throw new NotImplementedException();
    }
}
