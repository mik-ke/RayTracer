using RayTracer.Extensions.Shapes;
using System.Dynamic;

namespace RayTracer.Models;

/// <summary>
/// A Sphere shape. Assumed to originate in the object origin (0, 0, 0).
/// Also assumed to be a unit sphere with a radius of one.
/// </summary>
public sealed class Sphere : Shape
{
    private static int _currentIDCounter = 0;

    public int ID { get; init; }

    public static readonly Point Origin = new(0, 0, 0);

    public Sphere() : this(null)
    {
    }

    public Sphere(Matrix? transform) : base(transform)
    {
        ID = _currentIDCounter++;
    }

    protected override Intersections LocalIntersect(Ray localRay)
    {
        Vector sphereToRay = localRay.Origin - new Point(0, 0, 0);

        double a = localRay.Direction.Dot(localRay.Direction);
        double b = 2 * localRay.Direction.Dot(sphereToRay);
        double c = sphereToRay.Dot(sphereToRay) - 1;

        double discriminant = Math.Pow(b, 2) - 4 * a * c;
        if (discriminant < 0) return Intersections.Empty;

        double t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
        double t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

        Intersection intersection1 = new(t1, this);
        Intersection intersection2 = new(t2, this);

        return new Intersections(intersection1, intersection2);
    }

    protected override Vector LocalNormal(Point localPoint)
    {
        return localPoint - Origin;
    }

    #region equality
    public override bool Equals(object? obj) => Equals(obj as Sphere);

    public bool Equals(Sphere? other)
    {
        return other is not null && other.ID == ID;
    }

    public override int GetHashCode() => ID.GetHashCode();
    #endregion
}
