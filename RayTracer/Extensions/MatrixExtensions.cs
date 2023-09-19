using RayTracer.Models;

namespace RayTracer.Extensions;

public static class MatrixExtensions
{
    /// <summary>
    /// Multiplies the given <see cref="Matrix"/> by a 4x4 x rotation matrix built according to <paramref name="rotationRadians"/>.
    /// </summary>
    public static Matrix RotateX(this Matrix matrix, double rotationRadians)
    {
        return matrix * Matrix.RotationX(rotationRadians);
    }

    /// <summary>
    /// Multiplies the given <see cref="Matrix"/> by a 4x4 y rotation matrix built according to <paramref name="rotationRadians"/>.
    /// </summary>
    public static Matrix RotateY(this Matrix matrix, double rotationRadians)
    {
        return matrix * Matrix.RotationY(rotationRadians);
    }

    /// <summary>
    /// Multiplies the given <see cref="Matrix"/> by a 4x4 z rotation matrix built according to <paramref name="rotationRadians"/>.
    /// </summary>
    public static Matrix RotateZ(this Matrix matrix, double rotationRadians)
    {
        return matrix * Matrix.RotationZ(rotationRadians);
    }

    /// <summary>
    /// Multiplies the given <see cref="Matrix"/> by a 4x4 translation matrix built according to <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/>.
    /// </summary>
    public static Matrix Translate(this Matrix matrix, int x, int y, int z)
    {
        return matrix * Matrix.Translation(x, y, z);
    }

    /// <summary>
    /// Multiplies the given <see cref="Matrix"/> by a 4x4 scaling matrix built according to <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/>.
    /// </summary>
    public static Matrix Scale(this Matrix matrix, int x, int y, int z)
    {
        return matrix * Matrix.Scaling(x, y, z);
    }

    /// <summary>
    /// Multiplies the given <see cref="Matrix"/> by a 4x4 shearing matrix built according to the _ in proportion to _ parameters.
    /// </summary>
    public static Matrix Shear(this Matrix matrix, int xy, int xz, int yx, int yz, int zx, int zy)
    {
        return matrix * Matrix.Shearing(xy, xz, yx, yz, zx, zy);
    }
}
