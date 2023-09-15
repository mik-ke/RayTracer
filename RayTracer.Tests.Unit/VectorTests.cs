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
        // Act
        Models.Tuple vector = new Vector(4.3, -4.2, 3.1);

        // Assert
        Assert.Equal(4.3, vector.X);
        Assert.Equal(-4.2, vector.Y);
        Assert.Equal(3.1, vector.Z);
        Assert.Equal(0.0, vector.W);
        Assert.False(vector is Point);
        Assert.True(vector is Vector);
    }

    #region operators
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
    #endregion
}
