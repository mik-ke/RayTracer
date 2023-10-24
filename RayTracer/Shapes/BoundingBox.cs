using RayTracer.Models;

namespace RayTracer.Shapes;

/// <summary>
/// Axis-aligned bounding box.
/// </summary>
public class BoundingBox
{
    /// <summary>
    /// The minimum point on the box.
    /// </summary>
    /// <remarks>Where the x, y and z coordinates are the smallest.</remarks>
    public Point Minimum { get; }

    /// <summary>
    /// The maximum point on the box.
    /// </summary>
    /// <remarks>Where the x, y and z coordinates are the largest.</remarks>
    public Point Maximum { get;  }

    public BoundingBox(Point minimum, Point maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public BoundingBox() : this(
        new Point(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity),
        new Point(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity))
    {
    }

    /// <summary>
    /// "Adds" the given <paramref name="point"/> to the <see cref="BoundingBox"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="Minimum"/> and <see cref="Maximum"/> are adjusted depending on the given <paramref name="point"/>.
    /// If any component of <paramref name="point"/> is less than the component of <see cref="Minimum"/> then it will be replaced.
    /// Similarly, if any component of <paramref name="point"/> is greater than the component of <see cref="Maximum"/> then it will be replaced.
    /// </remarks>
    public void Add(Point point)
    {
        if (point.X < Minimum.X) Minimum.X = point.X;
        if (point.Y < Minimum.Y) Minimum.Y = point.Y;
        if (point.Z < Minimum.Z) Minimum.Z = point.Z;

        if (point.X > Maximum.X) Maximum.X = point.X;
        if (point.Y > Maximum.Y) Maximum.Y = point.Y;
        if (point.Z > Maximum.Z) Maximum.Z = point.Z;
    }
}
