using RayTracer.Models;

namespace RayTracer.Patterns;

/// <summary>
/// A pattern that linearly interpolates between the two <see cref="Color"/>s <see cref="A"/> and <see cref="B"/>.
/// </summary>
public class GradientPattern : Pattern
{
    public Color A { get; }
    public Color B { get; }

    public GradientPattern(Color a, Color b, Matrix? transform = null) : base(transform)
    {
        A = a;
        B = b;
    }

    public override Color PatternAt(Point point)
    {
        var distance = B - A;
        var fraction = point.X - Math.Floor(point.X);

        return A + distance * fraction;
    }
}
