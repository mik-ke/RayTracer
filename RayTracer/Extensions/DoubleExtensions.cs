namespace RayTracer.Extensions;

public static class DoubleExtensions
{
    public static readonly double EPSILON = 0.0001;
    public static bool IsEqualTo(this double a, double b)
    {
        if (double.IsNaN(a) && double.IsNaN(b))
            return true;

        return Math.Abs(a - b) < EPSILON;
    }
}
