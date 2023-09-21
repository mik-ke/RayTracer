namespace RayTracer.Models;

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
    public IReadOnlyList<double> Intersect(Ray ray)
    {
        // TODO
        return null!;
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
