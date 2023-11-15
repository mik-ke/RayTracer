namespace RayTracer.Shapes;

/// <summary>
/// Defines a <see cref="Shape"/> that can be divided into subgroups.
/// </summary>
public interface IDivisibleShape
{
    /// <summary>
    /// Divides the divisible <see cref="Shape"/> and its children into subgroups recursively.
    /// If the <see cref="Shape"/> has fewer children than the given <paramref name="threshold"/>,
    /// then <see cref="Shape"/> is not divided. However, its children may still be divided
    /// if they are an <see cref="IDivisibleShape"/>.
    /// </summary>
    public void Divide(int threshold);

    /// <summary>
    /// Resets the stored bounds of the <see cref="Shape"/> and its children recursively.
    /// </summary>
    public void ResetStoredBounds();
}
