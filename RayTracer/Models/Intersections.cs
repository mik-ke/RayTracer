using System.Collections;

namespace RayTracer.Models;

public class Intersections : IEnumerable<Intersection>
{
    private Intersection[] _intersections;

    /// <summary>
    /// Returns a new Intersections collection with an empty underlying array
    /// </summary>
    public static Intersections Empty => new Intersections(Array.Empty<Intersection>());

    public Intersections(params Intersection[] intersections)
    {
        _intersections = intersections;
    }

    public Intersection this[int index] { get => _intersections[index]; }

    public int Count => _intersections.Length;

    public IEnumerator<Intersection> GetEnumerator()
    {
        foreach (var intersection in _intersections)
            yield return intersection;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
