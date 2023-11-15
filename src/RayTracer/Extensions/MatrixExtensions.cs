using RayTracer.Models;

namespace RayTracer.Extensions;

public static class MatrixExtensions
{
    /// <summary>
    /// Multiplies a 4x4 x rotation matrix built according to <paramref name="rotationRadians"/> by the given <see cref="Matrix"/>.
    /// </summary>
    public static Matrix RotateX(this Matrix matrix, double rotationRadians)
    {
        return Matrix.RotationX(rotationRadians) * matrix;
    }

    /// <summary>
    /// Multiplies a 4x4 y rotation matrix built according to <paramref name="rotationRadians"/> by the given <see cref="Matrix"/>.
    /// </summary>
    public static Matrix RotateY(this Matrix matrix, double rotationRadians)
    {
        return Matrix.RotationY(rotationRadians) * matrix;
    }

    /// <summary>
    /// Multiplies a 4x4 z rotation matrix built according to <paramref name="rotationRadians"/> by the given <see cref="Matrix"/>.
    /// </summary>
    public static Matrix RotateZ(this Matrix matrix, double rotationRadians)
    {
        return Matrix.RotationZ(rotationRadians) * matrix;
    }

    /// <summary>
    /// Multiplies a 4x4 translation matrix built according to <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> by the given <see cref="Matrix"/>.
    /// </summary>
    public static Matrix Translate(this Matrix matrix, double x, double y, double z)
    {
        return Matrix.Translation(x, y, z) * matrix;
    }

    /// <summary>
    /// Multiplies a 4x4 scaling matrix built according to <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> by the given <see cref="Matrix"/>.
    /// </summary>
    public static Matrix Scale(this Matrix matrix, double x, double y, double z)
    {
        return Matrix.Scaling(x, y, z) * matrix;
    }

    /// <summary>
    /// Multiplies a 4x4 shearing matrix built according to the _ in proportion to _ parameters by the given <see cref="Matrix"/>.
    /// </summary>
    public static Matrix Shear(this Matrix matrix, double xy, double xz, double yx, double yz, double zx, double zy)
    {
        return Matrix.Shearing(xy, xz, yx, yz, zx, zy) * matrix;
    }
}
