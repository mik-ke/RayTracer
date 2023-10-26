using RayTracer.Extensions;
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
    /// Merges the given <paramref name="other"/> to the <see cref="BoundingBox"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="BoundingBox"/> is resized sufficiently to contain the given <paramref name="other"/>.
    /// </remarks>
    public void Add(BoundingBox other)
    {
        Add(other.Minimum);
        Add(other.Maximum);
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

    /// <summary>
    /// Returns true if the <see cref="BoundingBox"/> contains the given <paramref name="point"/>.
    /// </summary>
    public bool Contains(Point point)
    {
        if (point.X < Minimum.X || point.X > Maximum.X) return false;
        if (point.Y < Minimum.Y || point.Y > Maximum.Y) return false;
        if (point.Z < Minimum.Z || point.Z > Maximum.Z) return false;
        return true;
    }

    /// <summary>
    /// Returns true if the <see cref="BoundingBox"/> contains the given <paramref name="other"/> box.
    /// </summary>
    public bool Contains(BoundingBox other)
    {
        if (other.Minimum.X < Minimum.X || other.Maximum.X > Maximum.X) return false;
        if (other.Minimum.Y < Minimum.Y || other.Maximum.Y > Maximum.Y) return false;
        if (other.Minimum.Z < Minimum.Z || other.Maximum.Z > Maximum.Z) return false;
        return true;
    }

    /// <summary>
    /// Transforms the <see cref="BoundingBox"/> by the given <paramref name="transform"/>.
    /// </summary>
    /// <returns>A new <see cref="BoundingBox"/>.</returns>
    public BoundingBox Transform(Matrix transform)
    {
        var points = new Point[]
        {
            Minimum,
            new Point(Minimum.X, Minimum.Y, Maximum.Z),
            new Point(Minimum.X, Maximum.Y, Minimum.Z),
            new Point(Minimum.X, Maximum.Y, Maximum.Z),
            new Point(Maximum.X, Minimum.Y, Minimum.Z),
            new Point(Maximum.X, Minimum.Y, Maximum.Z),
            new Point(Maximum.X, Maximum.Y, Minimum.Z),
            Maximum
        };

        BoundingBox transformedBox = new();
        foreach (var point in points)
            transformedBox.Add((Point)(transform * point));

        return transformedBox;
    }

    /// <summary>
    /// Returns true if the given <paramref name="ray"/> intersects the <see cref="BoundingBox"/>.
    /// </summary>
    public bool Intersects(Ray ray)
    {
        var (tMinX, tMaxX) = CheckAxis(ray.Origin.X, ray.Direction.X, Minimum.X, Maximum.X);
        var (tMinY, tMaxY) = CheckAxis(ray.Origin.Y, ray.Direction.Y, Minimum.Y, Maximum.Y);
        var (tMinZ, tMaxZ) = CheckAxis(ray.Origin.Z, ray.Direction.Z, Minimum.Z, Maximum.Z);

        var tMin = Math.Max(tMinX, Math.Max(tMinY, tMinZ));
        var tMax = Math.Min(tMaxX, Math.Min(tMaxY, tMaxZ));

        return tMin <= tMax;
    }

    /// <summary>
    /// Checks where the ray intersects the corresponding plane.
    /// Returns the minimum and maximum t values.
    /// </summary>
    private (double tMin, double tMax) CheckAxis(double origin, double direction, double min, double max)
    {
        double tMin, tMax;

        var tMinNumerator = min - origin;
        var tMaxNumerator = max - origin;

        if (Math.Abs(direction) >= DoubleExtensions.EPSILON)
        {
            tMin = tMinNumerator / direction;
            tMax = tMaxNumerator / direction;
        }
        else
        {
            tMin = tMinNumerator * double.PositiveInfinity;
            tMax = tMaxNumerator * double.PositiveInfinity;
        }

        if (tMin > tMax)
            return (tMax, tMin);

        return (tMin, tMax);
    }
}
