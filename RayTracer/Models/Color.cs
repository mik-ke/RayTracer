using RayTracer.Extensions;

namespace RayTracer.Models;

public sealed class Color
{
    public static Color Black = new Color(0, 0, 0);

    #region properties
    public double R { get; private set; }
    public double G { get; private set; }
    public double B { get; private set; }
    #endregion

    public Color(double r, double g, double b)
    {
        R = r;
        G = g;
        B = b;
    }

    #region arithmetic operators
    /// <summary>
    /// Adds a <see cref="Color"/> to another.
    /// </summary>
    /// <returns><paramref name="other"/> added to the <see cref="Color"/>.</returns>
    public Color Add(Color other)
    {
        return new(R + other.R, G + other.G, B + other.B);
    }
    public static Color operator +(Color left, Color right) => left.Add(right);

    /// <summary>
    /// Subtracts one <see cref="Color"/> from another.
    /// </summary>
    /// <returns><paramref name="other"/> subtracted from the <see cref="Color"/>.</returns>
    public Color Subtract(Color other)
    {
        return new(R - other.R, G - other.G, B - other.B);
    }
    public static Color operator -(Color left, Color right) => left.Subtract(right);

    /// <summary>
    /// Multiplies the <see cref="Color"/> by a scalar.
    /// </summary>
    /// <returns>The <see cref="Color"/> multiplied by <paramref name="scalar"/>.</returns>
    public Color Multiply(double scalar)
    {
        return new(R * scalar, G * scalar, B * scalar);
    }
    /// <summary>
    /// Multiplies the <see cref="Color"/> by another color.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>The <see cref="Color"/> multiplied by <paramref name="other"/>.</returns>
    public Color Multiply(Color other)
    {
        return new(R * other.R, G * other.G, B * other.B);
    }
    public static Color operator *(Color color, double scalar) => color.Multiply(scalar);
    public static Color operator *(Color leftColor, Color rightColor) => leftColor.Multiply(rightColor);
    #endregion

    #region equality
    public sealed override bool Equals(object? obj) => Equals(obj as Color);

    public bool Equals(Color? other)
    {
        return other is not null &&
            R.IsEqualTo(other.R) &&
            G.IsEqualTo(other.G) &&
            B.IsEqualTo(other.B);
    }

    public sealed override int GetHashCode() => HashCode.Combine(R, G, B);

    public static bool operator ==(Color? left, Color? right)
    {
        if (left is null && right is null) return true;
        if (left is null && right is not null) return false;
        return left!.Equals(right);
    }

    public static bool operator !=(Color? left, Color? right)
    {
        if (left is null && right is null) return false;
        if (left is null && right is not null) return true;
        return !left!.Equals(right);
    }
    #endregion
}
