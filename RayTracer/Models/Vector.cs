namespace RayTracer.Models;

public class Vector : Tuple
{
    public Vector(double x, double y, double z) : base(x, y, z, 0)
    {
    }

    #region operators
    /// <summary>
    /// Adds a <see cref="Vector"/> to another.
    /// </summary>
    public Vector Add(Vector other)
    {
        return new Vector(X +  other.X, Y + other.Y, Z + other.Z);
    }
    public static Vector operator +(Vector left, Vector right) => left.Add(right);

    /// <summary>
    /// Subtracts one <see cref="Vector"/> from another.
    /// </summary>
    public Vector Subtract(Vector other)
    {
        return new Vector(X - other.X, Y - other.Y, Z - other.Z);
    }
    public static Vector operator -(Vector left, Vector right) => left.Subtract(right);
    #endregion
}
