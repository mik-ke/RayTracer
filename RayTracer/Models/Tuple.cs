using RayTracer.Extensions;

namespace RayTracer.Models;

public abstract class Tuple
{
    #region properties
    public double X { get; private set; }
    public double Y { get; private set; }
    public double Z { get; private set; }
    public double W { get; private set; }
    #endregion

    public Tuple(double x, double y, double z, double w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    /// <summary>
    /// <see cref="Tuple"/> converted to a 4 x 1 <see cref="Matrix"/>.
    /// </summary>
    /// <param name="tuple"></param>
    public static implicit operator Matrix(Tuple tuple) => new Matrix(
        new double[4, 1]
        {
            { tuple.X },
            { tuple.Y },
            { tuple.Z },
            { tuple.W }
        });

    #region equality
    public sealed override bool Equals(object? obj) => Equals(obj as Tuple);

    public bool Equals(Tuple? other)
    {
        return other is not null &&
            X.IsEqualTo(other.X) &&
            Y.IsEqualTo(other.Y) &&
            Z.IsEqualTo(other.Z) &&
            W.IsEqualTo(other.W);
    }

    public sealed override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

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
