using RayTracer.Models;

namespace RayTracer.Comparers;

/// <summary>
/// Compares <see cref="Intersection"/>s according to their T values.
/// </summary>
public class IntersectionComparer : IComparer<Intersection>
{
    public int Compare(Intersection? x, Intersection? y)
    {
        if (x == null && y == null)
            return 0;
        if (x == null)
            return -1;
        if (y == null)
            return 1;

        return x.T.CompareTo(y.T);
    }
}
