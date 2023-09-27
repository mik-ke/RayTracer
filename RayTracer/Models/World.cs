namespace RayTracer.Models;

public sealed class World
{
    #region properties
    public PointLight? LightSource { get; init; } 
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
}
