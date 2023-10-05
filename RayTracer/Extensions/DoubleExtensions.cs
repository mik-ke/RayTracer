namespace RayTracer.Extensions;

public static class DoubleExtensions
{
    public static readonly double EPSILON = 0.0001;
    public static bool IsEqualTo(this double a, double b)
    {
        return Math.Abs(a - b) < EPSILON;
    }
}
