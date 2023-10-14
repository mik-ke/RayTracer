using RayTracer.Models;
using System.Collections;

namespace RayTracer.Shapes;

/// <summary>
/// A container for multiple <see cref="Shape"/>s.
/// </summary>
public sealed class Group : Shape, IEnumerable<Shape>
{
    private readonly List<Shape> _shapes;

    public Group(Matrix? transform = null) : base(transform)
    {
        _shapes = new();
    }

    /// <summary>
    /// Adds the given <paramref name="child"/> to the <see cref="Group"/>
    /// and sets the <paramref name="child"/>'s <see cref="Shape.Parent"/> as the <see cref="Group"/>.
    /// </summary>
    public void AddChild(Shape child)
    {
        _shapes.Add(child);
        child.Parent = this;
    }

    /// <summary>
    /// The number of <see cref="Shape"/>s in the <see cref="Group"/>.
    /// </summary>
    public int Count => _shapes.Count;

    protected override Intersections LocalIntersect(Ray localRay)
    {
        if (_shapes.Count == 0) return Intersections.Empty;

        var intersections = GetAllIntersections(localRay).ToArray();
        return new Intersections(intersections);
    }

    private IEnumerable<Intersection> GetAllIntersections(Ray ray)
    {
        foreach (var shape in _shapes)
            foreach (var intersection in shape.Intersect(ray))
                yield return intersection;
    }

    protected override Vector LocalNormal(Point localPoint)
    {
        throw new NotImplementedException();
    }

    #region IEnumerable
    public IEnumerator<Shape> GetEnumerator()
    {
        foreach (var shape in _shapes)
            yield return shape;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion
}
