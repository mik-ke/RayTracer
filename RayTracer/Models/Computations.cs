using RayTracer.Extensions;

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
    public Sphere Object { get; init; }

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
    /// Eye vector.
    /// </summary>
    public Vector EyeVector { get; init; }

    /// <summary>
    /// Surface normal vector.
    /// </summary>
    public Vector NormalVector { get; init; }

    /// <summary>
    /// Is the intersection inside the Object?
    /// </summary>
    public bool IsInside { get; init; }

    public Computations(Intersection intersection, Ray ray)
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
    }
}
