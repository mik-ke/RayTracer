using RayTracer.Models;

namespace RayTracer.Patterns;

/// <summary>
/// A pattern that alternates between the <see cref="Color"/>s <see cref="A"/> and <see cref="B"/>
/// on the x-axis.
/// </summary>
public class StripePattern
{
    public Color A { get; }
    public Color B { get; }

    public StripePattern(Color a, Color b)
    {
        A = a;
        B = b;
    }

    /// <summary>
    /// Returns the stripe at the given <paramref name="point"/>
    /// </summary>
    /// <returns>The <see cref="Color"/> of the stripe.</returns>
    public Color StripeAt(Point point)
    {
        if (Math.Floor(point.X) % 2 == 0) return A;
        return B;
    }
}
