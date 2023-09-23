using RayTracer.Comparers;
using System.Collections;

namespace RayTracer.Models;

/// <summary>
/// A collection type that contains <see cref="Intersection"/> objects.
/// </summary>
public sealed class Intersections : IEnumerable<Intersection>
{
    private readonly Intersection[] _intersections;

    /// <summary>
    /// Returns an empty Intersections collection.
    /// </summary>
    public static Intersections Empty => new Intersections(Array.Empty<Intersection>());

    public Intersections(params Intersection[] intersections)
    {
        _intersections = intersections;
        if (intersections.Length > 1)
            Array.Sort(intersections, new IntersectionComparer());
    }

    public Intersection this[int index] { get => _intersections[index]; }

    public int Length => _intersections.Length;

    /// <summary>
    /// Returns the "hit"; the <see cref="Intersection"/> with the lowest non-negative t value.
    /// </summary>
    public Intersection? Hit()
    {
        foreach (var intersection in _intersections)
            if (intersection.T >= 0) return intersection;

        return null;
    }

    #region IEnumerable
    public IEnumerator<Intersection> GetEnumerator()
    {
        foreach (var intersection in _intersections)
            yield return intersection;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}
