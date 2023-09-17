﻿using RayTracer.Extensions;
using System.Text;

namespace RayTracer.Models;

/// <summary>
/// Represents a matrix data structure
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
    /// Creates a <paramref name="numRows"/> x <paramref name="numColumns"/> matrix data structure with default double values.
    /// </summary>
    public Matrix(int numRows, int numColumns)
    {
        _matrix = new double[numRows, numColumns];
        NumberOfRows = numRows;
        NumberOfColumns = numColumns;
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
        if (row < 0 || row >= NumberOfRows)
            throw new ArgumentOutOfRangeException(nameof(row));
        if (column < 0 || column >= NumberOfColumns)
            throw new ArgumentOutOfRangeException(nameof(column));
        return _matrix[row, column];
    }

    /// <summary>
    /// Sets the value of the matrix at the <paramref name="row"/> and <paramref name="column"/> to the given <paramref name="value"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when row or column index is less than 0 or greater than or equal to the number of rows or columns, respectively.</exception>
    public void SetValue(int row, int column, double value)
    {
        if (row < 0 || row >= NumberOfRows)
            throw new ArgumentOutOfRangeException(nameof(row));
        if (column < 0 || column >= NumberOfColumns)
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
        Matrix identity = new(NumberOfColumns, NumberOfColumns);
        for (int i = 0; i < NumberOfColumns; i++)
        {
            identity[i, i] = 1;
        }
        return identity;
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
