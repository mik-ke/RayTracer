namespace RayTracer.Models;

public sealed class Point : Tuple
{
    public Point(double x, double y, double z) : base(x, y, z, 1)
    {
    }

    #region arithmetic operations
    /// <summary>
    /// Adds a <see cref="Vector"/> to the <see cref="Point"/>.
    /// </summary>
    /// <returns>The <paramref name="vector"/> added to the <see cref="Point"/>.</returns>
    public Point Add(Vector vector)
    {
        return new Point(X + vector.X, Y + vector.Y, Z + vector.Z);
    }
    public static Point operator +(Point point, Vector vector) => point.Add(vector);

    /// <summary>
    /// Subtracts a <see cref="Point"/> from another.
    /// </summary>
    /// <returns><paramref name="other"/> subtracted from the <see cref="Point"/>.</returns>
    public Vector Subtract(Point other)
    {
        return new Vector(X - other.X, Y - other.Y, Z - other.Z);
    }
    /// <summary>
    /// Subtracts a <see cref="Vector"/> from the <see cref="Point"/>.
    /// </summary>
    /// <returns>The <paramref name="vector"/> subtracted from the <see cref="Point"/>.</returns>
    public Point Subtract(Vector vector)
    {
        return new Point(X - vector.X, Y - vector.Y, Z - vector.Z);
    }
    public static Point operator -(Point point, Vector vector) => point.Subtract(vector);
    public static Vector operator -(Point leftPoint, Point rightPoint) => leftPoint.Subtract(rightPoint);
    #endregion

    /// <summary>
    /// Converts a <see cref="Matrix"/> of size 4x1 to a <see cref="Point"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidCastException">Thrown when the matrix is not of size 4x1.</exception>
    /// <exception cref="InvalidCastException">Thrown when the matrix's [0, 3] value is not 1 (the w component).</exception>
    public static explicit operator Point(Matrix matrix)
    {
        if (matrix == null) throw new ArgumentNullException(nameof(matrix));
        if (matrix.NumberOfColumns != 1 || matrix.NumberOfRows != 4)
            throw new InvalidCastException("Cannot cast Matrix that is not of size 4x1!");
        if (matrix[3, 0] != 1)
            throw new InvalidCastException("The W component must be 1!");
        return new(matrix[0, 0], matrix[1, 0], matrix[2, 0]);
    }

    public override string ToString() => $"Point({X}, {Y}, {Z})";
}
