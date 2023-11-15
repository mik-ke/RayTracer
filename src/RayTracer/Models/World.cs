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

        Computations computations = new(hit, ray, intersections);
        return ShadeHit(computations, remainingRecursions);
    }

    /// <summary>
    /// Returns a collection of t values where the given <paramref name="ray"/> intersects
    /// with the <see cref="Objects"/>.
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

            Color reflected = ReflectedColor(computations, remainingRecursions);
            Color refracted = RefractedColor(computations, remainingRecursions);

            if (computations.Object.Material.IsReflectiveAndTransparent())
            {
                var reflectance = computations.Shlick();
                finalColor += reflected * reflectance +
                    refracted * (1 - reflectance);
            }
            else
            {
                finalColor += reflected + refracted;
            }
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
        if (remainingRecursions < 1)
            return Color.Black;
        if (computations.Object.Material.Reflective == 0)
            return Color.Black;

        Ray reflect = new(computations.OverPoint, computations.ReflectVector);
        Color color = ColorAt(reflect, remainingRecursions - 1);

        return color * computations.Object.Material.Reflective;
    }

    public Color RefractedColor(Computations computations, int remainingRecursions = DEFAULT_RECURSIONS)
    {
        if (remainingRecursions < 1)
            return Color.Black;
        if (computations.Object.Material.Transparency == 0)
            return Color.Black;

        var nRatio = computations.N1 / computations.N2;
        var cosI = computations.EyeVector.Dot(computations.NormalVector);
        var sin2T = nRatio * nRatio * (1 - cosI * cosI);

        if (Computations.IsTotalInternalReflection(sin2T))
            return Color.Black;

        var cosT = Math.Sqrt(1.0 - sin2T);
        Vector refractDirection = computations.NormalVector * (nRatio * cosI - cosT)
            - computations.EyeVector * nRatio;
        Ray refractRay = new(computations.UnderPoint, refractDirection);
        Color color = ColorAt(refractRay, remainingRecursions - 1);

        return color * computations.Object.Material.Transparency;
    }

    /// <summary>
    /// Resets the stored bounding box of each <see cref="Group"/> contained within the <see cref="World"/>.
    /// Called after rendering.
    /// </summary>
    public void ResetStoredBounds()
    {
        foreach (var obj in Objects)
            if (obj is IDivisibleShape divisible)
            {
                divisible.ResetStoredBounds();
            }
    }
}
