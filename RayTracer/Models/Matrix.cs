namespace RayTracer.Models;

public class Matrix
{
    #region fields
    double[,] _matrix;
    #endregion

    /// <summary>
    /// Creates a <paramref name="numRows"/> x <paramref name="numColumns"/> matrix data structure.
    /// </summary>
    public Matrix(int numRows, int numColumns)
    {
        _matrix = new double[numRows, numColumns];
    }


}
