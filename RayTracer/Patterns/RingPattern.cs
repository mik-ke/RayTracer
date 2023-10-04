using RayTracer.Models;

namespace RayTracer.Patterns;

/// <summary>
/// A ring pattern. Depends on two dimensions x and z.
/// </summary>
public class RingPattern : Pattern
{
    public Color A { get; }
    public Color B { get; }

    public RingPattern(Color a, Color b, Matrix? transform = null) : base(transform)
    {
        A = a;
        B = b;
    }

    public override Color PatternAt(Point point)
    {
        var distance = Math.Sqrt(point.X * point.X + point.Z * point.Z);
        if (Math.Floor(distance) % 2 == 0) return A;
        return B;
    }
}
