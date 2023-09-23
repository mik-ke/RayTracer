namespace RayTracer.Models;

/// <summary>
/// A Sphere shape. Assumed to originate in the world origin (0, 0, 0).
/// Also assumed to be a unit sphere with a radius of one.
/// </summary>
public sealed class Sphere
{
    private static int _currentIDCounter = 0;

    public int ID { get; init; }

    /// <summary>
    /// The transform of the sphere. I.e. the conversion from object space (unit sphere) to world space.
    /// 4x4 Identity matrix by default.
    /// </summary>
    public Matrix Transform { get; set; } = Matrix.Identity(4);

    public Sphere()
    {
        ID = _currentIDCounter++;
    }

    /// <summary>
    /// Returns a collection of t values where the given <paramref name="ray"/> intersects the <see cref="Sphere"/>.
    /// </summary>
    public Intersections Intersect(Ray ray)
    {
        Ray transformedRay = ray.Transform(Transform.Inverse());

        Vector sphereToRay = transformedRay.Origin - new Point(0, 0, 0);

        double a = transformedRay.Direction.Dot(transformedRay.Direction);
        double b = 2 * transformedRay.Direction.Dot(sphereToRay);
        double c = sphereToRay.Dot(sphereToRay) - 1;

        double discriminant = Math.Pow(b, 2) - 4 * a * c;
        if (discriminant < 0) return Intersections.Empty;

        double t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
        double t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

        Intersection intersection1 = new(t1, this);
        Intersection intersection2 = new(t2, this);

        return new Intersections(intersection1, intersection2);
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
