using RayTracer.Extensions;

namespace RayTracer.Models;

/// <summary>
/// Encapsulates the surface color and the four attributes of the Phong reflection model;
/// ambient, diffuse, specular and shininess.
/// </summary>
public sealed class Material
{
    public Color Color { get; set; }
    public double Ambient { get; set; }
    public double Diffuse { get; set; }
    public double Specular { get; set; }
    public double Shininess { get; set; }

    public Material()
    {
        Color = new Color(1, 1, 1);
        Ambient = 0.1;
        Diffuse = 0.9;
        Specular = 0.9;
        Shininess = 200.0;
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
