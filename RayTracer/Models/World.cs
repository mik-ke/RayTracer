using System.Runtime.CompilerServices;
using RayTracer.Shapes;

namespace RayTracer.Models;

public sealed class World
{
    private const int DEFAULT_RECURSIONS = 4;

    #region properties
    public List<PointLight> LightSources { get; } = new List<PointLight>();
    public List<Shape> Objects { get; } = new List<Shape>();
    #endregion

    /// <summary>
    /// Intersects the given <paramref name="ray"/> with the <see cref="World"/>
    /// and returns the color at the resulting intersection.
    /// </summary>
    public Color ColorAt(Ray ray, int remainingRecursions = DEFAULT_RECURSIONS)
    {
        var intersections = Intersect(ray);
        var hit = intersections.Hit();
        if (hit == null) return Color.Black;

        Computations computations = new(hit, ray);
        return ShadeHit(computations, remainingRecursions);
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
    public Color ShadeHit(Computations computations, int remainingRecursions = DEFAULT_RECURSIONS)
    {
        Color finalColor = Color.Black;

        foreach (var light in LightSources)
        {
            bool isShadowed = IsShadowed(computations.OverPoint, light);
            finalColor += computations.Object.Material.Lighting(
                computations.Object,
                light,
                computations.OverPoint,
                computations.EyeVector,
                computations.NormalVector,
                isShadowed);

            Color reflect = ReflectedColor(computations, remainingRecursions);
            finalColor += reflect;
        }

        return finalColor;
    }

    /// <summary>
    /// Returns true if the given <paramref name="point"/> in the <see cref="World"/> is in shadow
    /// for the given <paramref name="light"/>.
    /// </summary>
    public bool IsShadowed(Point point, PointLight light)
    {
        var pointToLight = light.Position - point;
        var distance = pointToLight.Magnitude();
        var direction = pointToLight.Normalize();

        Ray ray = new(point, direction);
        var intersections = Intersect(ray);

        var hit = intersections.Hit();
        if (hit != null && hit.T < distance)
            return true;

        return false;
    }

    /// <summary>
    /// Returns the reflected <see cref="Color"/> of the given <paramref name="computations"/>
    /// </summary>
    /// <param name="remainingRecursions">The remaining recursions. If less than 1, returns without effect (i.e. black).</param>
    public Color ReflectedColor(Computations computations, int remainingRecursions = DEFAULT_RECURSIONS)
    {
        if (computations.Object.Material.Reflective == 0)
            return Color.Black;
        if (remainingRecursions < 1)
            return Color.Black;

        Ray reflect = new(computations.OverPoint, computations.ReflectVector);
        Color color = ColorAt(reflect, remainingRecursions - 1);

        return color * computations.Object.Material.Reflective;
    }
}
