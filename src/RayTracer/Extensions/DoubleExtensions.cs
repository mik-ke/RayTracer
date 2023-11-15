namespace RayTracer.Extensions;

public static class DoubleExtensions
{
    public static readonly double EPSILON = 0.0001;
    public static bool IsEqualTo(this double a, double b)
    {
        if (double.IsNaN(a) && double.IsNaN(b))
            return true;
        if (double.IsInfinity(a) && double.IsInfinity(b))
            return true;
        if (double.IsNegativeInfinity(a) && double.IsNegativeInfinity(b))
            return true;

        return Math.Abs(a - b) < EPSILON;
    }
}
