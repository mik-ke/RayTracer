using RayTracer.Models;

namespace RayTracer.Patterns;

/// <summary>
/// A pattern that alternates between the <see cref="Color"/>s <see cref="A"/> and <see cref="B"/>
/// on the x-axis.
/// </summary>
public sealed class StripePattern : Pattern
{
    public Color A { get; }
    public Color B { get; }

    public StripePattern(Color a, Color b, Matrix? transform = null) : base(transform)
    {
        A = a;
        B = b;
    }

    public override Color PatternAt(Point point)
    {
        if (Math.Floor(point.X) % 2 == 0) return A;
        return B;
    }
}
