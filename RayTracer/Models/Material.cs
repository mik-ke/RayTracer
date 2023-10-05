using RayTracer.Extensions;
using RayTracer.Patterns;
using RayTracer.Shapes;

namespace RayTracer.Models;

/// <summary>
/// Encapsulates the surface color and the four attributes of the Phong reflection model;
/// ambient, diffuse, specular and shininess.
/// </summary>
public sealed class Material
{
    public Color Color { get; set; }
    public Pattern? Pattern { get; set; }
    public double Ambient { get; set; }
    public double Diffuse { get; set; }
    public double Specular { get; set; }
    public double Shininess { get; set; }
    public double Reflective { get; set; }

    public Material()
    {
        Color = new Color(1, 1, 1);
        Ambient = 0.1;
        Diffuse = 0.9;
        Specular = 0.9;
        Shininess = 200.0;
    }

    /// <summary>
    /// Defines what <see cref="Models.Color"/> shades the <paramref name="object"/> so it appears three-dimensional.
    /// Calculated by combining the ambient, diffuse and specular contributions.
    /// </summary>
    /// <param name="inShadow">
    /// Determines if the point is in shadow. If true, ignores diffuse and specular components.
    /// </param>
    /// <returns>A new <see cref="Models.Color"/></returns>
    public Color Lighting(Shape @object, PointLight light, Point position,
        Vector eye, Vector normal, bool inShadow = false)
    {
        var color = Color;
        if (Pattern != null) color = Pattern.PatternAtShape(@object, position);
        Color effective = color * light.Intensity;
        Vector lightDirection = (light.Position - position).Normalize();
        Color ambient = effective * Ambient;

        if (inShadow) return ambient;

        // light dot normal is the cosine of the angle between the light vector and surface normal vector
        // negative number means the light is on the other side of the surface
        double lightDotNormal = lightDirection.Dot(normal);
        if (lightDotNormal < 0)
            return ambient + Color.Black + Color.Black;

        Color diffuse = effective * Diffuse * lightDotNormal;
        // reflect dot eye represents the cosine of the angel between the reflection vector and eye vector
        // negative number means the light reflects away from the eye
        Vector reflect = -lightDirection.Reflect(normal);
        double reflectDotEye = reflect.Dot(eye);
        if (reflectDotEye <= 0)
            return ambient + diffuse + Color.Black;

        var factor = Math.Pow(reflectDotEye, Shininess);
        Color specular = light.Intensity * Specular * factor;

        return ambient + diffuse + specular;
    }

    #region equality
    public override bool Equals(object? obj) => Equals(obj as Material);

    public bool Equals(Material? other)
    {
        return other is not null
            && other.Color == Color
            && other.Ambient.IsEqualTo(Ambient)
            && other.Diffuse.IsEqualTo(Diffuse)
            && other.Specular.IsEqualTo(Specular)
            && other.Shininess.IsEqualTo(Shininess);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Color, Ambient, Diffuse, Specular, Shininess);
    }
    #endregion
}
