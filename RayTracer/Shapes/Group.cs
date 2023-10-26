﻿using RayTracer.Models;
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
