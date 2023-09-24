using RayTracer.Models;

namespace RayTracer.Tests.Unit;

public class VectorTests
{
    [Fact]
    public void Constructor_ShouldHaveWZero_WhenCreated()
    {
        // Arrange
        // Act
        Models.Tuple vector = new Vector(4.3, -4.2, 3.1);

        // Assert
        Assert.Equal(0.0, vector.W);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectProperties_WhenCreated()
    {
        // Arrange
        const double x = 4.3;
        const double y = -4.2;
        const double z = 3.1;

        // Act
        Models.Tuple vector = new Vector(x, y, z);

        // Assert
        Assert.Equal(x, vector.X);
        Assert.Equal(y, vector.Y);
        Assert.Equal(z, vector.Z);
        Assert.Equal(0.0, vector.W);
        Assert.False(vector is Point);
        Assert.True(vector is Vector);
    }

    #region arithmetic operations
    [Fact]
    public void Add_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Vector leftVector = new(3, -2, 5);
        Vector rightVector = new(-2, 3, 1);
        Vector expected = new(1, 1, 6);

        // Act
        Vector actual = leftVector.Add(rightVector);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void PlusOperator_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Vector leftVector = new(3, -2, 5);
        Vector rightVector = new(-2, 3, 1);
        Vector expected = new(1, 1, 6);

        // Act
        Vector actual = leftVector + rightVector;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Subtract_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Vector leftVector = new(3, 2, 1);
        Vector rightVector = new(5, 6, 7);
        Vector expected = new(-2, -4, -6);

        // Act
        Vector actual = leftVector.Subtract(rightVector);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MinusOperator_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Vector leftVector = new(3, 2, 1);
        Vector rightVector = new(5, 6, 7);
        Vector expected = new(-2, -4, -6);

        // Act
        Vector actual = leftVector - rightVector;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Subtract_ShouldBeCorrect_WhenSubtractingFromZeroVector()
    {
        // Arrange
        Vector vector = new(1, -2, 3);
        Vector expected = new(-1, 2, -3);

        // Act
        Vector actual = Vector.Zero - vector;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Negate_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Vector vector = new(1, -2, 3);
        Vector expected = new(-1, 2, -3);

        // Act
        Vector actual = vector.Negate();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void NegateOperator_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Vector vector = new(1, -2, 3);
        Vector expected = new(-1, 2, -3);

        // Act
        Vector actual = -vector;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Multiply_ShouldBeCorrect_WhenMultipliedByScalar()
    {
        // Arrange
        const double scalar = 3.5;
        Vector vector = new(1, -2, 3);
        Vector expected = new(3.5, -7, 10.5);

        // Act
        Vector actual = vector.Multiply(scalar);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MultiplyOperator_ShouldBeCorrect_WhenMultipliedByScalar()
    {
        // Arrange
        const double scalar = 3.5;
        Vector vector = new(1, -2, 3);
        Vector expected = new(3.5, -7, 10.5);

        // Act
        Vector actual = vector * scalar;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Multiply_ShouldBeCorrect_WhenMultipliedByFraction()
    {
        // Arrange
        const double fraction = 0.5;
        Vector vector = new(1, -2, 3);
        Vector expected = new(0.5, -1, 1.5);

        // Act
        Vector actual = vector.Multiply(fraction);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MultiplyOperator_ShouldBeCorrect_WhenMultipliedByFraction()
    {
        // Arrange
        const double fraction = 0.5;
        Vector vector = new(1, -2, 3);
        Vector expected = new(0.5, -1, 1.5);

        // Act
        Vector actual = vector * fraction;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Divide_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        const double divisor = 2;
        Vector vector = new(1, -2, 3);
        Vector expected = new(0.5, -1, 1.5);

        // Act
        Vector actual = vector.Divide(divisor);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DivideOperator_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        const double divisor = 2;
        Vector vector = new(1, -2, 3);
        Vector expected = new(0.5, -1, 1.5);

        // Act
        Vector actual = vector / divisor;

        // Assert
        Assert.Equal(expected, actual);
    }
    #endregion

    [Theory]
    [MemberData(nameof(MagnitudeData))]
    public void Magnitude_ShouldBeCorrect_WhenCalled(Vector vector, double expected)
    {
        // Arrange
        // Act
        double actual = vector.Magnitude();

        // Assert
        Assert.Equal(expected, actual);
    }
    public static IEnumerable<object[]> MagnitudeData =>
        new List<object[]>
        {
            new object[] { new Vector(1, 0, 0), 1 },
            new object[] { new Vector(0, 1, 0), 1 },
            new object[] { new Vector(0, 0, 1), 1 },
            new object[] { new Vector(1, 2, 3), Math.Sqrt(14) },
            new object[] { new Vector(-1, -2, -3), Math.Sqrt(14) }
        };

    [Theory]
    [MemberData(nameof(NormalizingData))]
    public void Normalize_ShouldBeCorrect_WhenCalled(Vector vectorToNormalize, Vector expected)
    {
        // Arrange
        // Act
        Vector actual = vectorToNormalize.Normalize();

        // Assert
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> NormalizingData =>
        new List<object[]>
        {
            new object[] { new Vector(4, 0, 0), new Vector(1, 0, 0) },
            new object[] { new Vector(1, 2, 3), new Vector(1 / Math.Sqrt(14), 2 / Math.Sqrt(14), 3 / Math.Sqrt(14)) }
        };

    [Fact]
    public void Normalize_ShouldBeOfMagnitudeOne_WhenCalled()
    {
        // Arrange
        const double expected = 1;
        Vector vector = new(1, 2, 3);

        // Act
        double actual = vector.Normalize().Magnitude();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Dot_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        const double expected = 20;
        Vector v1 = new(1, 2, 3);
        Vector v2 = new(2, 3, 4);

        // Act
        double actual = v1.Dot(v2);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(CrossExpectedData))]
    public void Cross_ShouldBeCorrect_WhenCalled(Vector v1, Vector v2, Vector expected)
    {
        // Arrange
        // Act
        Vector actual = v1.Cross(v2);

        // Assert
        Assert.Equal(expected, actual);
    }
    public static IEnumerable<object[]> CrossExpectedData =>
        new List<object[]>
        {
            new object[] { new Vector(1, 2, 3), new Vector(2, 3, 4), new Vector(-1, 2, -1) },
            new object[] { new Vector(2, 3, 4), new Vector(1, 2, 3), new Vector(1, -2, 1) }
        };

    #region cast from matrix
    [Fact]
    public void ExplicitCastFromMatrix_ShouldWork_WhenValidMatrixUsed()
    {
        // Arrange
        Matrix matrix = new(
            new double[4, 1]
            {
                { 1 },
                { 2 },
                { 3 },
                { 0 },
            });
        Vector expected = new(1, 2, 3);

        // Act
        var actual = (Vector)matrix;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ExplicitCastFromMatrix_ShouldThrowNullException_WhenMatrixIsNull()
    {
        // Arrange
        Matrix matrix = null!;

        // Act
        Exception e = Record.Exception(() => (Vector)matrix);

        // Assert
        Assert.IsType<ArgumentNullException>(e);
    }

    [Fact]
    public void ExplicitCastFromMatrix_ShouldThrowInvalidCastException_WhenMatrixIsWrongSize()
    {
        // Arrange
        Matrix matrix = new(1, 4);

        // Act
        Exception e = Record.Exception(() => (Vector)matrix);

        // Assert
        Assert.IsType<InvalidCastException>(e);
    }

    [Fact]
    public void ExplicitCastFromMatrix_ShouldThrowInvalidCastException_WhenMatrixHasWrongWComponent()
    {
        // Arrange
        Matrix matrix = new(4, 1);
        matrix[3, 0] = 1;

        // Act
        Exception e = Record.Exception(() => (Vector)matrix);

        // Assert
        Assert.IsType<InvalidCastException>(e);
    }
    #endregion
}
