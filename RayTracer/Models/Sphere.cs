namespace RayTracer.Models;

/// <summary>
/// A Sphere shape. Assumed to originate in the world origin (0, 0, 0).
/// Also assumed to be a unit sphere with a radius of one.
/// </summary>
public sealed class Sphere
{
    private static int _currentIDCounter = 0;

    public int ID { get; init; }

    public Sphere()
    {
        ID = _currentIDCounter++;
    }

    /// <summary>
    /// Returns a collection of t values where the given <paramref name="ray"/> intersects the <see cref="Sphere"/>.
    /// </summary>
    public double[] Intersect(Ray ray)
    {
        Vector sphereToRay = ray.Origin - new Point(0, 0, 0);

        double a = ray.Direction.Dot(ray.Direction);
        double b = 2 * ray.Direction.Dot(sphereToRay);
        double c = sphereToRay.Dot(sphereToRay) - 1;

        double discriminant = Math.Pow(b, 2) - 4 * a * c;
        if (discriminant < 0) return Array.Empty<double>();

        double t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
        double t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

        return new double[2] { t1, t2 };
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
