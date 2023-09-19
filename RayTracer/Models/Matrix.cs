using RayTracer.Extensions;
using System.Text;

namespace RayTracer.Models;

/// <summary>
/// Represents a matrix data structure.
/// </summary>
public sealed class Matrix
{
    #region fields
    double[,] _matrix;
    #endregion

    #region properties
    public int NumberOfRows { get; init; }
    public int NumberOfColumns { get; init; }
    #endregion

    /// <summary>
    /// Returns a new identity <see cref="Matrix"/> of the given <paramref name="size"/>.
    /// </summary>
    public static Matrix Identity(int size)
    {
        Matrix identity = new Matrix(size, size);
        for (int i = 0; i < size; i++)
        {
            identity[i, i] = 1;
        }
        return identity;
    }

    #region transformations
    /// <summary>
    /// Returns a new 4x4 translation <see cref="Matrix"/> with the given values <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/>.
    /// </summary>
    public static Matrix Translation(int x, int y, int z)
    {
        return new Matrix(
            new double[4, 4]
            {
                { 1, 0, 0, x },
                { 0, 1, 0, y },
                { 0, 0, 1, z },
                { 0, 0, 0, 1 }
            });
    }

    /// <summary>
    /// Returns a new 4x4 scaling <see cref="Matrix"/> with the given values <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/>.
    /// </summary>
    public static Matrix Scaling(int x, int y, int z)
    {
        return new Matrix(
            new double[4, 4]
            {
                { x, 0, 0, 0 },
                { 0, y, 0, 0 },
                { 0, 0, z, 0 },
                { 0, 0, 0, 1 },
            });
    }

    /// <summary>
    /// Returns a new 4x4 <see cref="Matrix"/> for shear transformation based on the _ in proportion to _ parameters.
    /// </summary>
    public static Matrix Shearing(int xy, int xz, int yx, int yz, int zx, int zy)
    {
        return new Matrix(
            new double[4, 4]
            {
                { 1,  xy, xz, 0 },
                { yx, 1,  yz, 0 },
                { zx, zy, 1,  0 },
                { 0,  0,  0,  1}
            });
    }
    #endregion

    #region rotation
    /// <summary>
    /// Returns a new 4x4 <see cref="Matrix"/> for rotating tuples <paramref name="rotationRadians"/> around the x axis.
    /// </summary>
    public static Matrix RotationX(double rotationRadians)
    {
        return new Matrix(
            new double[4, 4]
            {
                { 1, 0, 0, 0 },
                { 0, Math.Cos(rotationRadians), -Math.Sin(rotationRadians), 0 },
                { 0, Math.Sin(rotationRadians), Math.Cos(rotationRadians), 0 },
                { 0, 0, 0, 1 }
            });
    }

    /// <summary>
    /// Returns a new 4x4 <see cref="Matrix"/> for rotating tuples <paramref name="rotationRadians"/> around the y axis.
    /// </summary>
    public static Matrix RotationY(double rotationRadians)
    {
        return new Matrix(
            new double[4, 4]
            {
                { Math.Cos(rotationRadians), 0, Math.Sin(rotationRadians), 0 },
                { 0, 1, 0, 0},
                { -Math.Sin(rotationRadians), 0, Math.Cos(rotationRadians), 0 },
                { 0, 0, 0, 1 }
            });
    }

    /// <summary>
    /// Returns a new 4x4 <see cref="Matrix"/> for rotating tuples <paramref name="rotationRadians"/> around the z axis.
    /// </summary>
    public static Matrix RotationZ(double rotationRadians)
    {
        return new Matrix(
            new double[4, 4]
            {
                { Math.Cos(rotationRadians), -Math.Sin(rotationRadians), 0, 0 },
                { Math.Sin(rotationRadians), Math.Cos(rotationRadians), 0, 0 },
                { 0, 0, 1, 0},
                { 0, 0, 0, 1 }
            });
    }
    #endregion


    /// <summary>
    /// Creates a <paramref name="numberOfRows"/> x <paramref name="numberOfColumns"/> matrix data structure with default double values.
    /// </summary>
    public Matrix(int numberOfRows, int numberOfColumns)
    {
        _matrix = new double[numberOfRows, numberOfColumns];
        NumberOfRows = numberOfRows;
        NumberOfColumns = numberOfColumns;
    }

    /// <summary>
    /// Creates a matrix data structure from the given <paramref name="matrix"/> 2d array.
    /// </summary>
    /// <param name="matrix"></param>
    public Matrix(double[,] matrix)
    {
        _matrix = matrix;
        NumberOfRows = matrix.GetLength(0);
        NumberOfColumns = matrix.GetLength(1);
    }

    public double this[int row, int column]
    {
        get => GetValue(row, column);
        set => SetValue(row, column, value);
    }

    /// <summary>
    /// Get the value of the matrix at the <paramref name="row"/> and <paramref name="column"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when row or column index is less than 0 or greater than or equal to the number of rows or columns, respectively.</exception>
    public double GetValue(int row, int column)
    {
        if (!IsRowInbounds(row))
            throw new ArgumentOutOfRangeException(nameof(row));
        if (!IsColumnInbounds(column))
            throw new ArgumentOutOfRangeException(nameof(column));
        return _matrix[row, column];
    }

    /// <summary>
    /// Sets the value of the matrix at the <paramref name="row"/> and <paramref name="column"/> to the given <paramref name="value"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when row or column index is less than 0 or greater than or equal to the number of rows or columns, respectively.</exception>
    public void SetValue(int row, int column, double value)
    {
        if (!IsRowInbounds(row))
            throw new ArgumentOutOfRangeException(nameof(row));
        if (!IsColumnInbounds(column))
            throw new ArgumentOutOfRangeException(nameof(column));
        _matrix[row, column] = value;
    }

    #region arithmetic operations
    /// <summary>
    /// Returns a new <see cref="Matrix"/> that is the result of multiplying this by <paramref name="other"/>.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the number of rows of <paramref name="other"/> is not equal to the number of this matrix's columns.</exception>
    public Matrix Multiply(Matrix other)
    {
        if (NumberOfColumns != other.NumberOfRows)
            throw new ArgumentException($"Cannot multiply a matrix with {NumberOfColumns} columns by a matrix with unequal number of rows ({other.NumberOfRows})!", nameof(other));
        Matrix result = new(NumberOfRows, other.NumberOfColumns);
        for (int row = 0; row < NumberOfRows; row++)
            for (int column = 0; column < other.NumberOfColumns; column++)
                result[row, column] = GetRowColumnDotProduct(row, column, other);

        return result;
    }
    public static Matrix operator *(Matrix left, Matrix right) => left.Multiply(right);

    /// <summary>
    /// Returns the dot product of the given <paramref name="row"/> of this matrix and <paramref name="column"/> of <paramref name="other"/>.
    /// </summary>
    private double GetRowColumnDotProduct(int row, int column, Matrix other)
    {
        double dotProduct = 0;
        for (int i = 0; i < NumberOfColumns; i++)
            dotProduct += this[row, i] * other[i, column];

        return dotProduct;
    }
    #endregion

    /// <summary>
    /// Returns the identity matrix of the <see cref="Matrix"/>.
    /// </summary>
    public Matrix Identity()
    {
        return Identity(NumberOfColumns);
    }

    /// <summary>
    /// Returns the transpose of the <see cref="Matrix"/>.
    /// </summary>
    public Matrix Transpose()
    {
        Matrix transpose = new(NumberOfColumns, NumberOfRows);
        for (int row = 0; row < NumberOfRows; row++)
            for (int column = 0; column < NumberOfColumns; column++)
                transpose[column, row] = this[row, column];
        return transpose;
    }

    /// <summary>
    /// Returns the inverse of the <see cref="Matrix"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the <see cref="Matrix"/> is noninvertible.</exception>
    public Matrix Inverse()
    {
        if (!IsInvertible())
            throw new InvalidOperationException("Cannot invert noninvertible matrix!");

        int size = NumberOfRows;
        Matrix inverse = new(size, size);
        double determinant = Determinant();
        for (int row = 0; row < size; row++)
            for (int column = 0; column < size; column++)
            {
                double cofactor = Cofactor(row, column);

                inverse[column, row] = cofactor / determinant;
            }

        return inverse;
    }

    /// <summary>
    /// Checks if the matrix is invertible.
    /// </summary>
    /// <returns>True if invertible, false otherwise.</returns>
    public bool IsInvertible()
    {
        if (NumberOfRows != NumberOfColumns || NumberOfRows < 1) return false;

        return Determinant() != 0;
    }

    /// <summary>
    /// Returns the determinant of the <see cref="Matrix"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the <see cref="Matrix"/> is rectangular or of size 0.</exception>
    public double Determinant()
    {
        if (NumberOfRows != NumberOfColumns)
            throw new InvalidOperationException("Cannot calculate determinant for a rectangular matrix!");
        if (NumberOfRows < 1)
            throw new InvalidOperationException("Cannot calculate determinant for 0x0 matrix!");

        if (NumberOfRows == 2)
            return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];

        double determinant = 0;
        for (int column = 0; column < NumberOfColumns; column++)
            determinant += this[0, column] * Cofactor(0, column);

        return determinant;
    }

    /// <summary>
    /// Returns the submatrix of this <see cref="Matrix"/> after removing the given <paramref name="rowToRemove"/> and <paramref name="columnToRemove"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="rowToRemove"/> or <paramref name="columnToRemove"/> is one.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="rowToRemove"/> or <paramref name="columnToRemove"/> is out of bounds.</exception>
    public Matrix Submatrix(int rowToRemove, int columnToRemove)
    {
        if (NumberOfRows == 1 || NumberOfColumns == 1)
            throw new InvalidOperationException("Cannot create a submatrix from a matrix with a single row and/or column!");
        if (!IsRowInbounds(rowToRemove))
            throw new ArgumentOutOfRangeException(nameof(rowToRemove));
        if (!IsColumnInbounds(columnToRemove))
            throw new ArgumentOutOfRangeException(nameof(columnToRemove));

        Matrix submatrix = new(NumberOfRows - 1, NumberOfColumns - 1);
        bool rowRemoved = false;
        for (int row = 0; row < NumberOfRows; row++)
        {
            if (row == rowToRemove)
            {
                rowRemoved = true;
                continue;
            }

            bool columnRemoved = false;
            for (int column = 0; column < NumberOfColumns; column++)
            {
                if (column == columnToRemove)
                {
                    columnRemoved = true;
                    continue;
                }
                var submatrixRow = rowRemoved ? row - 1 : row;
                var submatrixColumn = columnRemoved ? column - 1 : column;
                submatrix[submatrixRow, submatrixColumn] = this[row, column];
            }
        }

        return submatrix;
    }

    /// <summary>
    /// Returns the minor of the <see cref="Matrix"/> at <paramref name="row"/>, <paramref name="column"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="row"/> or <paramref name="column"/> is out of bounds.</exception>
    public double Minor(int row, int column)
    {
        if (!IsRowInbounds(row))
            throw new ArgumentOutOfRangeException(nameof(row));
        if (!IsColumnInbounds(column))
            throw new ArgumentOutOfRangeException(nameof(column));

        return Submatrix(row, column).Determinant();
    }

    /// <summary>
    /// Returns the cofactor of the <see cref="Matrix"/> at <paramref name="row"/>, <paramref name="column"/>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public double Cofactor(int row, int column)
    {
        if (!IsRowInbounds(row))
            throw new ArgumentOutOfRangeException(nameof(row));
        if (!IsColumnInbounds(column))
            throw new ArgumentOutOfRangeException(nameof(column));

        var minor = Minor(row, column);
        bool isSignChanged = (row + column) % 2 != 0;
        if (isSignChanged) minor *= -1;

        return minor;
    }

    #region equality
    public override bool Equals(object? obj) => Equals(obj as Matrix);
    public bool Equals(Matrix? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        if (!HasSameDimensions(other))
            return false;

        return HasSameValues(other);
    }
    private bool HasSameValues(Matrix other)
    {
        for (int row = 0; row < NumberOfRows; row++)
            for (int column = 0;  column < NumberOfColumns; column++)
                if (!this[row, column].IsEqualTo(other[row, column]))
                    return false;

        return true;
    }
    /// <summary>
    /// Returns whether or not the <see cref="Matrix"/> and <paramref name="other"/> have the same dimensions.
    /// </summary>
    public bool HasSameDimensions(Matrix other)
    {
        return NumberOfRows == other.NumberOfRows
            && NumberOfColumns == other.NumberOfColumns;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();

        for (int row = 0; row < NumberOfRows; row++)
            for (int column = 0; column < NumberOfColumns; column++)
                hash.Add(this[row, column]);

        return hash.ToHashCode();
    }

    public static bool operator ==(Matrix? left, Matrix? right)
    {
        if (left is null && right is null) return true;
        if (left is null && right is not null) return false;
        return left!.Equals(right);
    }

    public static bool operator !=(Matrix? left, Matrix? right)
    {
        if (left is null && right is null) return false;
        if (left is null && right is not null) return true;
        return !left!.Equals(right);
    }
    #endregion

    /// <summary>
    /// Checks if <paramref name="row"/> is inbounds for the matrix.
    /// </summary>
    /// <return>True if inbounds, false otherwise</return>
    private bool IsRowInbounds(int row)
    {
        return row >= 0 && row < NumberOfRows;
    }

    /// <summary>
    /// Checks if <paramref name="column"/> is inbounds for the matrix.
    /// </summary>
    /// <return>True if inbounds, false otherwise</return>
    private bool IsColumnInbounds(int column)
    {
        return column >= 0 && column < NumberOfColumns;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();
        for (int row = 0; row < NumberOfRows; row++)
        {
            for (int column = 0; column < NumberOfColumns; column++)
            {
                stringBuilder.Append(this[row, column]);
                if (column != NumberOfColumns - 1) stringBuilder.Append(' ');
            }
            if (row != NumberOfRows - 1) stringBuilder.AppendLine();
        }
        return stringBuilder.ToString();
    }
}
