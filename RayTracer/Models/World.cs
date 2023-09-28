namespace RayTracer.Models;

public sealed class World
{
    #region properties
    public PointLight? LightSource { get; set; } 
    public List<Sphere> Objects { get; } = new List<Sphere>();
    #endregion

    /// <summary>
    /// Returns a collection of t values where the given <paramref name="ray"/> intersects
    /// with the <see cref="Objects"/>
    /// </summary>
    public Intersections Intersect(Ray ray)
    {
        var intersections = GetAllIntersections(ray).ToArray();
        return new Intersections(intersections);
    }

    private IEnumerable<Intersection> GetAllIntersections(Ray ray)
    {
        foreach (var obj in Objects)
            foreach (var intersection in obj.Intersect(ray))
                yield return intersection;
    }

    /// <summary>
    /// Computes and returns the shading of the given intersection's <paramref name="computations"/>.
    /// </summary>
    public Color ShadeHit(Computations computations)
    {
        if (LightSource == null) return Color.Black;

        return computations.Object.Material.Lighting(
            LightSource,
            computations.Point,
            computations.EyeVector,
            computations.NormalVector);
    }
}
