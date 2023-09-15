namespace RayTracer.Models;

public class Point : Tuple
{
    public Point(double x, double y, double z) : base(x, y, z, 1)
    {
    }

    #region operators
    /// <summary>
    /// Adds a <see cref="Vector"/> to a <see cref="Point"/>.
    /// </summary>
    public Point Add(Vector vector)
    {
        return new Point(X + vector.X, Y + vector.Y, Z + vector.Z);
    }
    public static Point operator +(Point point, Vector vector) => point.Add(vector);

    /// <summary>
    /// Subtracts a <see cref="Point"/> from another.
    /// </summary>
    public Vector Subtract(Point other)
    {
        return new Vector(X - other.X, Y - other.Y, Z - other.Z);
    }
    /// <summary>
    /// Subtracts a <see cref="Vector"/> from a <see cref="Point"/>
    /// </summary>
    public Point Subtract(Vector vector)
    {
        return new Point(X - vector.X, Y - vector.Y, Z - vector.Z);
    }
    public static Point operator -(Point point, Vector vector) => point.Subtract(vector);
    public static Vector operator -(Point leftPoint, Point rightPoint) => leftPoint.Subtract(rightPoint);
    #endregion
}
