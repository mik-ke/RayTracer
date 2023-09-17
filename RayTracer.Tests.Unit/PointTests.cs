using RayTracer.Models;
using Xunit;

namespace RayTracer.Tests.Unit;

public class PointTests
{
    [Fact]
    public void Constructor_ShouldHaveWOne_WhenCreated()
    {
        // Arrange
        // Act
        Models.Tuple point = new Point(4.3, -4.2, 3.1);

        // Assert
        Assert.Equal(1.0, point.W);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectProperties_WhenCreated()
    {
        // Arrange
        const double x = 4.3;
        const double y = -4.2;
        const double z = 3.1;

        // Act
        Models.Tuple point = new Point(x, y, z);

        // Assert
        Assert.Equal(x, point.X);
        Assert.Equal(y, point.Y);
        Assert.Equal(z, point.Z);
        Assert.Equal(1.0, point.W);
        Assert.True(point is Point);
        Assert.False(point is Vector);
    }

    #region arithmetic operations
    [Fact]
    public void Add_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Point point = new(3, -2, 5);
        Vector vector = new (-2, 3, 1);
        Point expected = new(1, 1, 6);

        // Act
        Point actual = point.Add(vector);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void PlusOperator_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Point point = new(3, -2, 5);
        Vector vector = new (-2, 3, 1);
        Point expected = new(1, 1, 6);

        // Act
        Point actual = point + vector;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Subtract_ShouldBeCorrect_WhenPointSubtracted()
    {
        // Arrange
        Point leftPoint = new(3, 2, 1);
        Point rightPoint = new(5, 6, 7);
        Vector expected = new(-2, -4, -6);

        // Act
        Vector actual = leftPoint.Subtract(rightPoint);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MinusOperator_ShouldBeCorrect_WhenPointSubtracted()
    {
        // Arrange
        Point leftPoint = new(3, 2, 1);
        Point rightPoint = new(5, 6, 7);
        Vector expected = new(-2, -4, -6);

        // Act
        Vector actual = leftPoint - rightPoint;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Subtract_ShouldBeCorrect_WhenVectorSubtracted()
    {
        // Arrange
        Point point = new(3, 2, 1);
        Vector vector = new(5, 6, 7);
        Point expected = new Point(-2, -4, -6);

        // Act
        Point actual = point.Subtract(vector);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MinusOperator_ShouldBeCorrect_WhenVectorSubtracted()
    {
        // Arrange
        Point point = new(3, 2, 1);
        Vector vector = new(5, 6, 7);
        Point expected = new(-2, -4, -6);

        // Act
        Point actual = point - vector;

        // Assert
        Assert.Equal(expected, actual);
    }
    #endregion
}
