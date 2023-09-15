using RayTracer.Extensions;

namespace RayTracer.Models;

public class Tuple
{
    #region properties
    public double X { get; private set; }
    public double Y { get; private set; }
    public double Z { get; private set; }
    public double W { get; private set; }
    #endregion

    #region factories
    public static Tuple Point(double x, double y, double z) => new(x, y, z, 1.0);
    public static Tuple Vector(double x, double y, double z) => new(x, y, z, 0.0);
    #endregion

    public Tuple(double x, double y, double z, double w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public bool IsPoint() => W == 1.0;
    public bool IsVector() => W == 0.0;

    #region operators
    public Tuple Add(Tuple other) => new Tuple(
        X + other.X,
        Y + other.Y,
        Z + other.Z,
        W + other.W);

    public static Tuple operator +(Tuple left, Tuple right) => left.Add(right);
    #endregion

    #region equality
    public override bool Equals(object? obj) => Equals(obj as Tuple);

    public bool Equals(Tuple? other)
    {
        return other is not null &&
            X.IsEqualTo(other.X) &&
            Y.IsEqualTo(other.Y) &&
            Z.IsEqualTo(other.Z) &&
            W.IsEqualTo(other.W);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    public static bool operator ==(Tuple? left, Tuple? right)
    {
        if (left is null && right is null) return true;
        if (left is null && right is not null) return false;
        return left!.Equals(right);
    }

    public static bool operator !=(Tuple? left, Tuple? right)
    {
        if (left is null && right is null) return false;
        if (left is null && right is not null) return true;
        return !left!.Equals(right);
    }
    #endregion
}
