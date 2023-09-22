namespace RayTracer.Models;

public class Intersection
{
    /// <summary>
    /// t value of the intersection.
    /// </summary>
    public double T { get; init; }

    /// <summary>
    /// Object that was intersected.
    /// </summary>
    public Sphere Object { get; init; }

    public Intersection(double t, Sphere @object)
    {
        T = t;
        Object = @object;
    }
}
