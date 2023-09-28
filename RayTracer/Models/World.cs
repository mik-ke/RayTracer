namespace RayTracer.Models;

public sealed class World
{
    #region properties
    public List<PointLight> LightSources { get; } = new List<PointLight>();
    public List<Sphere> Objects { get; } = new List<Sphere>();
    #endregion

    /// <summary>
    /// Intersects the given <paramref name="ray"/> with the <see cref="World"/>
    /// and returns the color at the resulting intersection.
    /// </summary>
    public Color ColorAt(Ray ray)
    {
        var intersections = Intersect(ray);
        var hit = intersections.Hit();
        if (hit == null) return Color.Black;

        Computations computations = new(hit, ray);
        return ShadeHit(computations);
    }

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
        Color finalColor = Color.Black;

        foreach (var light in LightSources)
        {
            finalColor += computations.Object.Material.Lighting(
                light,
                computations.Point,
                computations.EyeVector,
                computations.NormalVector);
        }

        return finalColor;
    }
}
