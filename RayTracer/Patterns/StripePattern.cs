using RayTracer.Models;
using RayTracer.Shapes;

namespace RayTracer.Patterns;

/// <summary>
/// A pattern that alternates between the <see cref="Color"/>s <see cref="A"/> and <see cref="B"/>
/// on the x-axis.
/// </summary>
public class StripePattern
{
    public Color A { get; }
    public Color B { get; }
    public Matrix Transform { get; set; }

    public StripePattern(Color a, Color b, Matrix? transform = null)
    {
        A = a;
        B = b;
        Transform = transform ?? Matrix.Identity(4);
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

    /// <summary>
    /// Returns the stripe on the given <paramref name="object"/>
    /// at the given <paramref name="worldPoint"/>.
    /// </summary>
    /// <returns>The <see cref="Color"/> of the stripe.</returns>
    public Color StripeAtObject(Shape @object, Point worldPoint)
    {
        var objectPoint = (Point)(@object.Transform.Inverse() * worldPoint);
        var patternPoint = (Point)(Transform.Inverse() * objectPoint);

        return StripeAt(patternPoint);
    }
}
