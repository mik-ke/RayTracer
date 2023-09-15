using RayTracer.Models;

namespace RayTracer.Tests.Unit;

public class TupleTests
{
    [Fact]
    public void Tuple_ShouldBePoint_WhenWIsOne()
    {
        // Arrange
        // Act
        Models.Tuple tuple = new(4.3, -4.2, 3.1, 1.0);

        // Assert
        Assert.Equal(4.3, tuple.X);
        Assert.Equal(-4.2, tuple.Y);
        Assert.Equal(3.1, tuple.Z);
        Assert.Equal(1.0, tuple.W);
        Assert.True(tuple.IsPoint());
        Assert.False(tuple.IsVector());
    }

    [Fact]
    public void Tuple_ShouldBeVector_WhenWIsZero()
    {
        // Arrange
        // Act
        Models.Tuple tuple = new(4.3, -4.2, 3.1, 0.0);

        // Assert
        Assert.Equal(4.3, tuple.X);
        Assert.Equal(-4.2, tuple.Y);
        Assert.Equal(3.1, tuple.Z);
        Assert.Equal(0.0, tuple.W);
        Assert.False(tuple.IsPoint());
        Assert.True(tuple.IsVector());
    }

    [Fact]
    public void Point_ShouldHaveWOne_WhenCalled()
    {
        // Arrange
        // Act
        Models.Tuple tuple = Models.Tuple.Point(4, -4, 3);

        // Assert
        Assert.True(tuple.W == 1);
    }


    [Fact]
    public void Vector_ShouldHaveWZero_WhenCalled()
    {
        // Arrange
        // Act
        Models.Tuple tuple = Models.Tuple.Vector(4, -4, 3);

        // Assert
        Assert.True(tuple.W == 0);
    }

    [Fact]
    public void Add_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        var t1 = new Models.Tuple(3, -2, 5, 1);
        var t2 = new Models.Tuple(-2, 3, 1, 0);

        // Act
        Models.Tuple result = t1.Add(t2);

        // Assert
        Assert.Equal(1, result.X);
        Assert.Equal(1, result.Y);
        Assert.Equal(6, result.Z);
        Assert.Equal(1, result.W);
    }

    #region equality
    [Fact]
    public void Equals_ShouldBeTrue_WhenAllSame()
    {
        // Arrange
        var t1 = new Models.Tuple(1, 2, 3, 1);
        var t2 = new Models.Tuple(1, 2, 3, 1);

        // Act
        var result = t1 == t2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldBeTrue_WhenBothNull()
    {
        // Arrange
        Models.Tuple t1 = null!;
        Models.Tuple t2 = null!;

        // Act
        var result = t1 == t2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldBeFalse_WhenOnlyOneNull()
    {
        // Arrange
        Models.Tuple t1 = new Models.Tuple(1, 2, 3, 1);
        Models.Tuple t2 = null!;

        // Act
        var result = t1 == t2;

        // Assert
        Assert.False(result);
    }
    #endregion
}
