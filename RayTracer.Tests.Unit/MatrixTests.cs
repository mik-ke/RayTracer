﻿using RayTracer.Models;
using Xunit;
namespace RayTracer.Tests.Unit;

public class MatrixTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly_WhenCalledWithFourByFourArray()
	{
		// Arrange
		var matrixArray = new double[4, 4]
		{
			{ 1, 2, 3, 4 },
			{ 5.5, 6.5, 7.5, 8.5},
			{ 9, 10, 11, 12 },
			{ 13.5, 14.5, 15.5, 16.5 }
		};

		// Act
		Matrix matrix = new(matrixArray);

		// Assert
		Assert.Equal(1, matrix[0, 0]);
		Assert.Equal(4, matrix[0, 3]);
		Assert.Equal(5.5, matrix[1, 0]);
		Assert.Equal(7.5, matrix[1, 2]);
		Assert.Equal(11, matrix[2, 2]);
		Assert.Equal(13.5, matrix[3, 0]);
		Assert.Equal(15.5, matrix[3, 2]);
	}

	[Fact]
	public void Constructor_ShouldInitializeCorrectly_WhenCalledWithTwoByTwoArray()
	{
		// Arrange
		var matrixArray = new double[2, 2]
		{
			{ -3, 5 },
			{ 1, -2 }
		};

		// Act
		Matrix matrix = new(matrixArray);

		// Assert
		Assert.Equal(-3, matrix[0, 0]);
		Assert.Equal(5, matrix[0, 1]);
		Assert.Equal(1, matrix[1, 0]);
		Assert.Equal(-2, matrix[1, 1]);
	}

	[Fact]
	public void Constructor_ShouldInitializeCorrectly_WhenCalledWithThreeByThreeArray()
	{
		// Arrange
		var matrixArray = new double[3, 3]
		{
			{ -3, 5, 0},
			{ 1, -2 , -7},
			{ 0, 1, 1 }
		};

		// Act
		Matrix matrix = new(matrixArray);

		// Assert
		Assert.Equal(-3, matrix[0, 0]);
		Assert.Equal(-2, matrix[1, 1]);
		Assert.Equal(1, matrix[2, 2]);
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(10, 0)]
	[InlineData(0, -1)]
	[InlineData(0, 20)]
	public void GetValue_ShouldThrowArgumentOutOfRangeException_WhenRowOrColumnOutOfBounds(int row, int column)
	{
		// Arrange
		Matrix matrix = new(10, 20);

		// Act
		Exception e = Record.Exception(() => matrix.GetValue(row, column));

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(e);
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(10, 0)]
	[InlineData(0, -1)]
	[InlineData(0, 20)]
	public void SetValue_ShouldThrowArgumentOutOfRangeException_WhenRowOrColumnOutOfBounds(int row, int column)
	{
		// Arrange
		Matrix matrix = new(10, 20);

		// Act
		Exception e = Record.Exception(() => matrix.SetValue(row, column, 0));

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(e);
	}

	#region arithmetic operations
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void Multiply_ShouldBeCorrect_WhenCalledWithFourByFourMatrices(bool useOperator)
	{
		// Arrange
		Matrix left = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});
		Matrix right = new(
			new double[4, 4]
			{
				{ -2, 1, 2, 3 },
				{ 3, 2, 1, -1 },
				{ 4, 3, 6, 5 },
				{ 1, 2, 7, 8 }
			});
		Matrix expected = new(
			new double[4, 4]
			{
				{ 20, 22, 50, 48 },
				{ 44, 54, 114, 108 },
				{ 40, 58, 110, 102 },
				{ 16, 26, 46, 42 }
			});

		// Act
		Matrix actual;
		if (useOperator)
			actual = left * right;
		else
            actual = left.Multiply(right);

		// Assert
		Assert.Equal(expected, actual);
	}


	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void Multiply_ShouldThrowArgumentException_WhenNumberColumnsOfThisNotEqualToNumberRowsOfOther(bool useOperator)
	{
		// Arrange
		Matrix left = new(10, 20);
		Matrix right = new(10, 20);

		// Act
		Exception e = Record.Exception(() => {
			if (useOperator)
				return left * right;
			else
				return left.Multiply(right);
        });

		// Assert
		Assert.IsType<ArgumentException>(e);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void Multiply_ShouldBeCorrect_WhenMultipliedByTuple(bool useOperator)
	{
		// Arrange
		Matrix matrix = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 2, 4, 4, 2 },
				{ 8, 6, 4, 1 },
				{ 0, 0, 0, 1 }
			});
		Point point = new(1, 2, 3);
		Point expected = new(18, 24, 33);

		// Act
		Matrix actual;
		if (useOperator)
			actual = matrix * point;
		else
			actual = matrix.Multiply(point);

		// Assert
		Assert.Equal(expected, actual);
	}
	#endregion

	[Fact]
	public void Identity_ShouldBeCorrect_WhenCalledWithSquareMatrix()
	{
		// Arrange
		Matrix matrix = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});
		Matrix expected = new(
			new double[4, 4]
			{
				{ 1, 0, 0, 0 },
				{ 0, 1, 0, 0 },
				{ 0, 0, 1, 0 },
				{ 0, 0, 0, 1 }
			});

		// Act
		var actual = matrix.Identity();

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Identity_ShouldBeCorrect_WhenCalledWithRectangularMatrix()
	{
		// Arrange
		Matrix matrix = new(
			new double[2, 3]
			{
				{ 1, 2, 4 },
				{ 2, 3, 5 },
			});
		Matrix expected = new(
			new double[3, 3]
			{
				{ 1, 0, 0 },
				{ 0, 1, 0 },
				{ 0, 0, 1 },
			});

		// Act
		var actual = matrix.Identity();

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Multiply_ShouldReturnSame_WhenSquareMatrixMultipliedByIdentity()
	{
		// Arrange
		Matrix matrix = new(
			new double[4, 4]
			{
				{ 0, 1, 2, 4 },
				{ 1, 2, 4, 8 },
				{ 2, 4, 8, 16 },
				{ 4, 8, 16, 32 }
			});
		Matrix identity = matrix.Identity();

		// Act
		var actual = matrix.Multiply(identity);

		// Assert
		Assert.Equal(matrix, actual);
	}

	[Fact]
	public void Multiply_ShouldReturnSame_WhenRectangularMatrixMultipliedByIdentity()
	{
		// Arrange
		Matrix matrix = new(
			new double[2, 3]
			{
				{ 1, 2, 4 },
				{ 2, 3, 5 },
			});
		Matrix identity = matrix.Identity();

		// Act
		var actual = matrix.Multiply(identity);

		// Assert
		Assert.Equal(matrix, actual);
	}

	[Fact]
	public void Transpose_ShouldBeCorrect_WhenCalled()
	{
		// Arrange
		Matrix matrix = new(
			new double[4, 4]
			{
				{ 0, 9, 3, 0 },
				{ 9, 8, 0, 8 },
				{ 1, 8, 5, 3 },
				{ 0, 0, 5, 8 }
			});
		Matrix expected = new(
			new double[4, 4]
			{
				{ 0, 9, 1, 0 },
				{ 9, 8, 8, 0 },
				{ 3, 0, 5, 5 },
				{ 0, 8, 3, 8 }
			});

		// Act
		var actual = matrix.Transpose();

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Transpose_ShouldReturnIdentityMatrix_WhenCalledOnIdentityMatrix()
	{
		// Arrange
		Matrix matrix = new(
			new double[4, 4]
			{
				{ 0, 9, 3, 0 },
				{ 9, 8, 0, 8 },
				{ 1, 8, 5, 3 },
				{ 0, 0, 5, 8 }
			});
		var identity = matrix.Identity();

		// Act
		var actual = identity.Transpose();

		// Assert
		Assert.Equal(identity, actual);
	}

	[Fact]
	public void Determinant_ShouldWork_WhenCalledForTwoByTwoMatrix()
	{
		// Arrange
		Matrix matrix = new(
			new double[2, 2]
			{
				{ 1, 5 },
				{ -3, 2 }
			});
		const double expected = 17;

		// Act
		var actual = matrix.Determinant();

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Determinant_ShouldWork_WhenCalledForThreeByThreeMatrix()
	{
		// Arrange
		Matrix matrix = new(
			new double[3, 3]
			{
				{ 1, 2, 6 },
				{ -5, 8, -4 },
				{ 2, 6, 4 }
			});
		const double expected = -196;

		// Act
		var actual = matrix.Determinant();

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Determinant_ShouldWork_WhenCalledForFourByFourMatrix()
	{
		// Arrange
		Matrix matrix = new(
			new double[4, 4]
			{
				{ -2, -8, 3, 5 },
				{ -3, 1, 7, 3 },
				{ 1, 2, -9, 6 },
				{ -6, 7, 7, -9 }
			});
		const double expected = -4071;

		// Act
		var actual = matrix.Determinant();

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Determinant_ShouldThrowInvalidOperationException_WhenCalledForRectangularMatrix()
	{
		// Arrange
		Matrix matrix = new(4, 5);

		// Act
		Exception e = Record.Exception(() => matrix.Determinant());

		// Assert
		Assert.IsType<InvalidOperationException>(e);
	}

	[Fact]
	public void Submatrix_ShouldReturnCorrectTwoByTwoMatrix_WhenCalledForThreeByThreeMatrix()
	{
		// Arrange
		Matrix matrix = new(
			new double[3, 3]
			{
				{ 1, 5, 0 },
				{ -3, 2, 7 },
				{ 0, 6, -3 }
			});
		Matrix expected = new(
			new double[2, 2]
			{
				{ -3, 2 },
				{ 0, 6 }
			});
		const int rowToRemove = 0;
		const int columnToRemove = 2;

		// Act
		var actual = matrix.Submatrix(rowToRemove, columnToRemove);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Submatrix_ShouldReturnCorrectThreeByThreeMatrix_WhenCalledForFourByFourMatrix()
	{
		// Arrange
		Matrix matrix = new(
			new double[4, 4]
			{
				{ -6, 1, 1, 6 },
				{ -8, 5, 8, 6 },
				{ -1, 0, 8, 2 },
				{ -7, 1, -1, 1 }
			});
		Matrix expected = new(
			new double[3, 3]
			{
				{ -6, 1, 6 },
				{ -8, 8, 6 },
				{ -7, -1, 1 }
			});
		const int rowToRemove = 2;
		const int columnToRemove = 1;

		// Act
		var actual = matrix.Submatrix(rowToRemove, columnToRemove);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(1, 2)]
	[InlineData(2, 1)]
	[InlineData(1, 1)]
	public void Submatrix_ShouldThrowInvalidOperationException_WhenMatrixHasOneRowAndOrColumn(int numberOfRows, int numberOfColumns)
	{
		// Arrange
		Matrix matrix = new(numberOfRows, numberOfColumns);

		// Act
		Exception e = Record.Exception(() => matrix.Submatrix(0, 0));

		// Assert
		Assert.IsType<InvalidOperationException>(e);
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(10, 0)]
	[InlineData(0, -1)]
	[InlineData(0, 20)]
	public void Submatrix_ShouldThrowArgumentOutOfRangeException_WhenRowAndOrColumnOutOfBounds(int row, int column)
	{
		// Arrange
		Matrix matrix = new(10, 20);

		// Act
		Exception e = Record.Exception(() => matrix.Submatrix(row, column));

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(e);
	}

	[Fact]
	public void Minor_ShouldBeCorrect_WhenCalled()
	{
		// Arrange
		Matrix matrix = new(
			new double[3, 3]
			{
				{ 3, 5, 0 },
				{ 2, -1, -7 },
				{ 6, -1, 5 }
			});
		const int row = 1;
		const int column = 0;
		double expected = 25;

		// Act
		double actual = matrix.Minor(row, column);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Minor_ShouldBeEqualToDeterminantOfSubmatrix_WhenCalledForSameRowColumn()
	{
		// Arrange
		Matrix matrix = new(
			new double[3, 3]
			{
				{ 3, 5, 0 },
				{ 2, -1, -7 },
				{ 6, -1, 5 }
			});
		const int row = 1;
		const int column = 0;
		var submatrix = matrix.Submatrix(row, column);
		var determinant = submatrix.Determinant();

		// Act
		double actual = matrix.Minor(row, column);

		// Assert
		Assert.Equal(determinant, actual);
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(10, 0)]
	[InlineData(0, -1)]
	[InlineData(0, 20)]
	public void Minor_ShouldThrowArgumentOutOfRangeException_WhenRowAndOrColumnOutOfBounds(int row, int column)
	{
		// Arrange
		Matrix matrix = new(10, 20);

		// Act
		Exception e = Record.Exception(() => matrix.Minor(row, column));

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(e);
	}

	[Theory]
	[InlineData(0, 0, -12)]
	[InlineData(1, 0, -25)]
	public void Cofactor_ShouldBeCorrect_WhenCalled(int row, int column, double expected)
	{
		// Arrange
		Matrix matrix = new(
			new double[3, 3]
			{
				{ 3, 5, 0 },
				{ 2, -1, -7 },
				{ 6, -1, 5 }
			});

		// Act
		var actual = matrix.Cofactor(row, column);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(10, 0)]
	[InlineData(0, -1)]
	[InlineData(0, 20)]
	public void Cofactor_ShouldThrowArgumentOutOfRangeException_WhenRowAndOrColumnOutOfBounds(int row, int column)
	{
		// Arrange
		Matrix matrix = new(10, 20);

		// Act
		Exception e = Record.Exception(() => matrix.Cofactor(row, column));

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(e);
	}

    #region equality
    [Fact]
	public void HasSameDimensions_ShouldReturnTrue_WhenSameRowAndColumnCount()
	{
		// Arrange
		Matrix matrix = new(10, 20);
		Matrix otherMatrix = new(10, 20);

		// Act
		bool hasSameDimensions = matrix.HasSameDimensions(otherMatrix);

		// Assert
		Assert.True(hasSameDimensions);
	}

	[Theory]
	[InlineData(11, 20)]
	[InlineData(10, 21)]
	[InlineData(11, 21)]
	public void HasSameDimensions_ShouldReturnFalse_WhenDifferentRowAndColumnCount(int otherNumRows, int otherNumColumns)
	{
		// Arrange
		Matrix matrix = new(10, 20);
		Matrix otherMatrix = new(otherNumRows, otherNumColumns);

		// Act
		bool hasSameDimensions = matrix.HasSameDimensions(otherMatrix);

		// Assert
		Assert.False(hasSameDimensions);
	}

	[Fact]
	public void Equals_ShouldReturnTrue_WithIdenticalMatrices()
	{
		// Arrange
		Matrix leftMatrix = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});
		Matrix rightMatrix = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});

		// Act
		bool result = leftMatrix == rightMatrix;

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void Equals_ShouldReturnFalse_WithDifferingMatrices()
	{
		// Arrange
		Matrix leftMatrix = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});
		Matrix rightMatrix = new(
			new double[4, 4]
			{
				{ 2, 3, 4, 5 },
				{ 6, 7, 8, 9 },
				{ 8, 7, 6, 5 },
				{ 4, 3, 2, 1 }
			});

		// Act
		bool result = leftMatrix == rightMatrix;

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void Equals_ShouldBeTrue_WhenBothNull()
	{
        // Arrange
        Matrix m1 = null!;
        Matrix m2 = null!;

        // Act
        var result = m1 == m2;

        // Assert
        Assert.True(result);
	}

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Equals_ShouldBeFalse_WhenOnlyOneNull(bool leftIsNull)
    {
        // Arrange
        Matrix m1, m2;
        if (leftIsNull)
        {
            m1 = null!;
			m2 = new(1, 1);
        }
        else
        {
			m1 = new(1, 1);
            m2 = null!;
        }

        // Act
        var result = m1 == m2;

        // Assert
        Assert.False(result);
    }

	[Fact]
	public void NotEqualOp_ShouldReturnTrue_WithDifferingMatrices()
	{
		// Arrange
		Matrix leftMatrix = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});
		Matrix rightMatrix = new(
			new double[4, 4]
			{
				{ 2, 3, 4, 5 },
				{ 6, 7, 8, 9 },
				{ 8, 7, 6, 5 },
				{ 4, 3, 2, 1 }
			});

		// Act
		bool result = leftMatrix != rightMatrix;

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void NotEqualOp_ShouldReturnFalse_WithIdenticalMatrices()
	{
		// Arrange
		Matrix leftMatrix = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});
		Matrix rightMatrix = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});

		// Act
		bool result = leftMatrix != rightMatrix;

		// Assert
		Assert.False(result);
	}

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
	public void NotEqualOp_ShouldBeTrue_WhenOnlyOneNull(bool leftIsNull)
	{
        // Arrange
        Matrix m1, m2;
        if (leftIsNull)
        {
            m1 = null!;
			m2 = new(1, 1);
        }
        else
        {
			m1 = new(1, 1);
            m2 = null!;
        }

        // Act
        var result = m1 != m2;

        // Assert
        Assert.True(result);
	}

	[Fact]
	public void NotEqualOp_ShouldBeFalse_WhenBothNull()
	{
        // Arrange
        Matrix m1 = null!;
        Matrix m2 = null!;

        // Act
        var result = m1 != m2;

        // Assert
        Assert.False(result);
	}

    [Fact]
    public void GetHashCode_ShouldBeEqual_WhenSameProperties()
    {
        // Arrange
        Models.Tuple t1 = new Point(1, 2, 3);
        Models.Tuple t2 = new Point(1, 2, 3);

        // Act
        var t1HashCode = t1.GetHashCode();
        var t2HashCode = t2.GetHashCode();

        // Assert
        Assert.True(t1HashCode == t2HashCode);
    }

	[Fact]
    public void GetHashCode_ShouldBeEqual_WhenSameValues()
    {
		// Arrange
		Matrix m1 = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});
		Matrix m2 = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});

		// Act
		var m1HashCode = m1.GetHashCode();
		var m2HashCode = m2.GetHashCode();

		// Assert
		Assert.Equal(m1HashCode, m2HashCode);
    }

	[Fact]
    public void GetHashCode_ShouldNotBeEqual_WhenDifferingValues()
    {
		// Arrange
		Matrix m1 = new(
			new double[4, 4]
			{
				{ 1, 2, 3, 4 },
				{ 5, 6, 7, 8 },
				{ 9, 8, 7, 6 },
				{ 5, 4, 3, 2 }
			});
		Matrix m2 = new(
			new double[4, 4]
			{
				{ 2, 3, 4, 5 },
				{ 6, 7, 8, 9 },
				{ 8, 7, 6, 5 },
				{ 4, 3, 2, 1 }
			});

		// Act
		var m1HashCode = m1.GetHashCode();
		var m2HashCode = m2.GetHashCode();

		// Assert
		Assert.NotEqual(m1HashCode, m2HashCode);
    }
    #endregion
}
