using RayTracer.Models;

namespace RayTracer.Patterns;

/// <summary>
/// A 3D checker pattern, i.e. repeating pattern of squares in three dimensions.
/// </summary>
public class CheckersPattern : Pattern
{
    public Color A { get; }
    public Color B { get; }

    public CheckersPattern(Color a, Color b, Matrix? transform = null) : base(transform)
    {
        A = a;
        B = b;
    }

    public override Color PatternAt(Point point)
    {
        var flooredSum = Math.Floor(point.X) + Math.Floor(point.Y) + Math.Floor(point.Z);
        if (flooredSum % 2 == 0) return A;
        return B;
    }
}
