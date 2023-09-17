using RayTracer.Models;
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

    #region arithmetic operators
    #endregion

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
