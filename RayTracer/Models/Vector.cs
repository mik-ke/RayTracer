namespace RayTracer.Models;

public sealed class Vector : Tuple
{
    #region properties
    /// <summary>
    /// The zero vector (x = y = z = 0). Used for negating.
    /// </summary>
    public static Vector Zero => new Vector(0, 0, 0);
    #endregion

    public Vector(double x, double y, double z) : base(x, y, z, 0)
    {
    }

    #region arithmetic operations
    /// <summary>
    /// Adds a <see cref="Vector"/> to another.
    /// </summary>
    /// <returns><paramref name="other"/> added to the <see cref="Vector"/>.</returns>
    public Vector Add(Vector other)
    {
        return new Vector(X +  other.X, Y + other.Y, Z + other.Z);
    }
    public static Vector operator +(Vector left, Vector right) => left.Add(right);

    /// <summary>
    /// Subtracts one <see cref="Vector"/> from another.
    /// </summary>
    /// <returns><paramref name="other"/> subtracted from the <see cref="Vector"/>.</returns>
    public Vector Subtract(Vector other)
    {
        return new Vector(X - other.X, Y - other.Y, Z - other.Z);
    }
    public static Vector operator -(Vector left, Vector right) => left.Subtract(right);

    public Vector Negate() => Zero - this;
    public static Vector operator -(Vector vector) => vector.Negate();

    /// <summary>
    /// Multiplies the <see cref="Vector"/> by a scalar.
    /// </summary>
    /// <returns>The <see cref="Vector"/> multiplied by the scalar <paramref name="multiplier"/>.</returns>
    public Vector Multiply(double multiplier)
    {
        return new Vector(X * multiplier, Y * multiplier, Z * multiplier);
    }
    public static Vector operator *(Vector vector, double multiplier) => vector.Multiply(multiplier);

    /// <summary>
    /// Divides the <see cref="Vector"/> by a scalar.
    /// </summary>
    /// <returns>The <see cref="Vector"/> divided by the scalar <paramref name="divisor"/>.</returns>
    public Vector Divide(double divisor)
    {
        return new Vector(X / divisor, Y / divisor, Z / divisor);
    }
    public static Vector operator /(Vector vector, double divisor) => vector.Divide(divisor);
    #endregion

    /// <summary>
    /// Returns the magnitude of the <see cref="Vector"/>.
    /// </summary>
    public double Magnitude()
    {
        return Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    /// <summary>
    /// Normalizes the <see cref="Vector"/>.
    /// </summary>
    /// <returns>The normalized <see cref="Vector"/> (new vector with a magnitude of one).</returns>
    public Vector Normalize()
    {
        var magnitude = Magnitude();
        return new Vector(X / magnitude, Y / magnitude, Z / magnitude);
    }

    /// <summary>
    /// Returns the dot product of the two <see cref="Vector"/>s.
    /// </summary>
    /// <returns>The dot product of this <see cref="Vector"/> and <paramref name="other"/>.</returns>
    public double Dot(Vector other)
    {
        return X * other.X +
            Y * other.Y +
            Z * other.Z;
    }

    /// <summary>
    /// Returns the cross product of the two <see cref="Vector"/>s.
    /// </summary>
    /// <param name="v2"></param>
    /// <returns></returns>
    public Vector Cross(Vector other)
    {
        return new Vector(
            Y * other.Z - Z * other.Y,
            Z * other.X - X * other.Z,
            X * other.Y - Y * other.X);
    }

    public override string ToString() => $"Vector({X}, {Y}, {Z})";
}
