﻿using RayTracer.Models;

namespace RayTracer.Shapes;

public abstract class Shape
{
    /// <summary>
    /// Parent <see cref="Group"/> of this shape, if any.
    /// </summary>
    public Group? Parent { get; set; }

    /// <summary>
    /// The transform of the shape. I.e. the conversion from object space (unit shape) to world space.
    /// 4x4 Identity matrix by default.
    /// </summary>
    public Matrix Transform { get; set; }

    /// <summary>
    /// The material of the shape.
    /// </summary>
    public Material Material { get; set; } = new Material();

    public Shape(Matrix? transform)
    {
        Transform = transform ?? Matrix.Identity(4);
    }
    public Shape() : this(null)
    {
    }

    /// <summary>
    /// Returns an <see cref="Intersections"/> collection built from where the given <paramref name="ray"/>
    /// intersects the <see cref="Shape"/>.
    /// </summary>
    public Intersections Intersect(Ray ray)
    {
        var localRay = ray.Transform(Transform.Inverse());
        return LocalIntersect(localRay);
    }

    /// <summary>
    /// Intersections functionality specific to the inheriting shape.
    /// </summary>
    /// <param name="localRay">The transformed ray.</param>
    /// <returns></returns>
    protected abstract Intersections LocalIntersect(Ray localRay);

    /// <summary>
    /// Returns the (surface) normal of the <see cref="Shape"/> at the given <paramref name="worldPoint"/>.
    /// </summary>
    /// <returns>A new <see cref="Vector"/>.</returns>
    public Vector Normal(Point worldPoint)
    {
        var localPoint = (Point)(Transform.Inverse() * worldPoint);
        var localNormal = LocalNormal(localPoint);
        // Technically we should be getting Transform.Submatrix(3, 3) as
        // any translations will screw the W value of a vector.
        // We can avoid that by simply using the normal 4x4 functionality
        // then setting the W to 0 afterwards.
        var worldNormal = Transform.Inverse().Transpose() * localNormal;
        worldNormal[3, 0] = 0;

        return ((Vector)worldNormal).Normalize();
    }

    /// <summary>
    /// Normal functionality specific to the inherting shape.
    /// </summary>
    /// <param name="localPoint">Transformed world point.</param>
    /// <returns></returns>
    protected abstract Vector LocalNormal(Point localPoint);
}
