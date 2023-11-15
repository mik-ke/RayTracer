using RayTracer.Models;
using RayTracer.Patterns;

namespace RayTracer.Tests.Unit;

/// <summary>
/// Used solely for testing abstract functionality of <see cref="Pattern"/>
/// </summary>
public class TestPattern : Pattern
{
    public TestPattern(Matrix? transform = null) : base(transform)
    {
    }

    public override Color PatternAt(Point point) => new(point.X, point.Y, point.Z);
}
