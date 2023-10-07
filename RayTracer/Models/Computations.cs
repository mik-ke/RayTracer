using RayTracer.Extensions;
using RayTracer.Shapes;

namespace RayTracer.Models;

/// <summary>
/// Data structure that encapsulates precomputed information related to an <see cref="Intersection"/>.
/// </summary>
public sealed record Computations
{
    /// <summary>
    /// Intersection t value.
    /// </summary>
    public double T { get; init; }

    /// <summary>
    /// Intersected object.
    /// </summary>
    public Shape Object { get; init; }

    /// <summary>
    /// Intersection position in world space.
    /// </summary>
    public Point Point { get; init; }

    /// <summary>
    /// The <see cref="Point"/> slightly adjusted in the direction of <see cref="NormalVector"/>.
    /// Used for preventing self-shadowing.
    /// </summary>
    public Point OverPoint { get; init; }

    /// <summary>
    /// The <see cref="Point"/> slightly adjusted in the negative direction of <see cref="NormalVector"/>.
    /// Used for refraction.
    /// </summary>
    public Point UnderPoint { get; init; }

    /// <summary>
    /// Eye vector.
    /// </summary>
    public Vector EyeVector { get; init; }

    /// <summary>
    /// Surface normal vector.
    /// </summary>
    public Vector NormalVector { get; init; }

    /// <summary>
    /// Ray's reflection vector.
    /// </summary>
    public Vector ReflectVector { get; init; }

    /// <summary>
    /// Is the intersection inside the Object?
    /// </summary>
    public bool IsInside { get; init; }

    /// <summary>
    /// Refractive index of the material the ray is passing from.
    /// </summary>
    public double N1 { get; set; }

    /// <summary>
    /// Refractive index of the material the ray is passing to.
    /// </summary>
    public double N2 { get; set; }

    public Computations(Intersection intersection, Ray ray, Intersections? intersections = null)
    {
        T = intersection.T;
        Object = intersection.Object;
        Point = ray.Position(T);
        EyeVector = -ray.Direction;
        NormalVector = Object.Normal(Point);

        if (NormalVector.Dot(EyeVector) < 0)
        {
            IsInside = true;
            NormalVector = -NormalVector;
        }

        OverPoint = Point + NormalVector * DoubleExtensions.EPSILON;
        UnderPoint = Point - NormalVector * DoubleExtensions.EPSILON;
        ReflectVector = ray.Direction.Reflect(NormalVector);

        if (intersections != null) InitializeNValues(intersection, intersections);
    }

    private void InitializeNValues(Intersection hit, Intersections intersections)
    {
        LinkedList<Shape> containers = new();
        foreach (Intersection intersection in intersections)
        {
            bool isHit = intersection == hit;
            if (isHit)
            {
                if (containers.Count < 1)
                    N1 = 1.0;
                else
                    N1 = containers.Last!.ValueRef.Material.RefractiveIndex;
            }

            if (containers.Contains(intersection.Object))
                containers.Remove(intersection.Object);
            else
                containers.AddLast(intersection.Object);

            if (isHit)
            {
                if (containers.Count < 1)
                    N2 = 1.0;
                else
                    N2 = containers.Last!.ValueRef.Material.RefractiveIndex;

                break;
            }
        }
    }

    /// <summary>
    /// Determines the reflectance and represents what fraction of the light is reflected
    /// given the surface information at the hit.
    /// </summary>
    /// <returns>A double between 0 and 1, inclusive, where 1 is total internal reflection.</returns>
    public double Shlick()
    {
        var cos = EyeVector.Dot(NormalVector);

        if (N1 > N2)
        {
            var nRatio = N1 / N2;
            var sin2T = nRatio * nRatio * (1.0 - cos * cos);
            if (sin2T > 1.0)
                return 1.0;

            var cosT = Math.Sqrt(1.0 - sin2T);
            cos = cosT;
        }

        var r0 = ((N1 - N2) / (N1 + N2)) * ((N1 - N2) / (N1 + N2));
        return r0 + (1 - r0) * Math.Pow(1 - cos, 5);
    }

    /// <summary>
    /// Total internal reflection means the ray's angle is so acute that it doesn't
    /// pass through the interface (i.e. shape). Calculated using Snell's Law.
    /// </summary>
    public static bool IsTotalInternalReflection(in double sin2T) => sin2T > 1;
}
