using RayTracer.Models;
using RayTracer.Shapes;

namespace RayTracer.Patterns;

/// <summary>
/// Pattern base class.
/// </summary>
public abstract class Pattern
{
    public Matrix Transform { get; set; }

    public Pattern(Matrix? transform = null)
    {
        Transform = transform ?? Matrix.Identity(4);
    }

    /// <summary>
    /// Returns the pattern on the given <paramref name="shape"/>
    /// at the given <paramref name="worldPoint"/>.
    /// </summary>
    /// <returns>The <see cref="Color"/> of the pattern.</returns>
    public Color PatternAtShape(Shape shape, Point worldPoint)
    {
        var objectPoint = shape.WorldToObject(worldPoint);
        var patternPoint = (Point)(Transform.Inverse() * objectPoint);

        return PatternAt(patternPoint);
    }

    /// <summary>
    /// Determines the pattern at the given <paramref name="point"/>.
    /// </summary>
    /// <returns>The <see cref="Color"/> of the <see cref="Pattern"/> at the given <paramref name="point"/>.</returns>
    public abstract Color PatternAt(Point point);
}
