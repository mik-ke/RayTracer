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

    public Group(IEnumerable<Shape> shapes, Matrix? transform = null) : base(transform)
    {
        _shapes = new();
        foreach (var shape in shapes)
            AddChild(shape);
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

    public Shape this[int index] => ChildAt(index);
    public Shape ChildAt(int index) => _shapes[index];

    /// <summary>
    /// The number of <see cref="Shape"/>s in the <see cref="Group"/>.
    /// </summary>
    public int Count => _shapes.Count;

    protected override Intersections LocalIntersect(Ray localRay)
    {
        if (_shapes.Count == 0) return Intersections.Empty;

        var boundingBox = BoundsOf();
        if (boundingBox.Intersects(localRay))
        {
            var intersections = GetAllIntersections(localRay).ToArray();
            return new Intersections(intersections);
        }
        else
        {
            return Intersections.Empty;
        }

    }

    private IEnumerable<Intersection> GetAllIntersections(Ray ray)
    {
        foreach (var shape in _shapes)
            foreach (var intersection in shape.Intersect(ray))
                yield return intersection;
    }

    public override BoundingBox BoundsOf()
    {
        BoundingBox boundingBox = new();

        foreach (var child in _shapes)
            boundingBox.Add(child.ParentSpaceBoundsOf());

        return boundingBox;
    }

    public (List<Shape> left, List<Shape> right) PartitionChildren()
    {
        var (left, right) = BoundsOf().SplitBounds();
        var leftShapes = new List<Shape>();
        var rightShapes = new List<Shape>();
        foreach (var child in _shapes)
        {
            var inLeft = left.Contains(child.ParentSpaceBoundsOf());
            var inRight = right.Contains(child.ParentSpaceBoundsOf());

            if (inLeft && inRight) continue;
            else if (inLeft) leftShapes.Add(child);
            else if (inRight) rightShapes.Add(child);
        }

        foreach (var shape in leftShapes) _shapes.Remove(shape);
        foreach (var shape in rightShapes) _shapes.Remove(shape);

        return (leftShapes, rightShapes);
    }

    /// <summary>
    /// Adds the given <paramref name="shapes"/> into a new subgroup
    /// and adds that subgroup to the <see cref="Group"/>.
    /// </summary>
    public void MakeSubgroup(IEnumerable<Shape> shapes)
    {
        Group subgroup = new(shapes);
        AddChild(subgroup);
    }

    /// <summary>
    /// Divides the <see cref="Group"/> and its children into subgroups recursively.
    /// If the <see cref="Group"/> has fewer children than the given <paramref name="threshold"/>,
    /// then the <see cref="Group"/> is not divided. However, its children may still be divided
    /// if they are a group and have more children than the given <paramref name="threshold"/>.
    /// </summary>
    public void Divide(int threshold)
    {
        if (threshold <= Count)
        {
            var (left, right) = PartitionChildren();
            if (left.Count > 0) MakeSubgroup(left);
            if (right.Count > 0) MakeSubgroup(right);
        }

        foreach (var shape in _shapes)
            if (shape is Group group)
                group.Divide(threshold);
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
