namespace RayTracer.Extensions;

public static class DoubleExtensions
{
    private const double EPSILON = 0.00001;
    public static bool IsEqualTo(this double a, double b)
    {
        if (Math.Abs(a - b) < EPSILON)
            return true;

        return false;
    }
}
