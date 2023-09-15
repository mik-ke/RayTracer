using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
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

    [Fact]
    public void PlusOperator_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        var t1 = new Models.Tuple(3, -2, 5, 1);
        var t2 = new Models.Tuple(-2, 3, 1, 0);

        // Act
        Models.Tuple result = t1 + t2;

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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Equals_ShouldBeFalse_WhenOnlyOneNull(bool leftIsNull)
    {
        // Arrange
        Models.Tuple t1, t2;
        if (leftIsNull)
        {
            t1 = null!;
            t2 = new(1, 2, 3, 1);
        }
        else
        {
            t1 = new(1, 2, 3, 1);
            t2 = null!;
        }

        // Act
        var result = t1 == t2;

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1, 2, 3, 1, 1, 2, 3, 0)]
    [InlineData(1, 2, 3, 1, 1, 2, 2, 1)]
    [InlineData(1, 2, 3, 1, 1, 1, 3, 1)]
    [InlineData(1, 2, 3, 1, 0, 2, 3, 1)]
    public void Equals_ShouldBeFalse_WhenNotAllSame(double x1, double y1, double z1, double w1,
        double x2, double y2, double z2, double w2)
    {
        // Arrange
        Models.Tuple t1 = new(x1, y1, z1, w1);
        Models.Tuple t2 = new(x2, y2, z2, w2);

        // Act
        var result = t1 == t2;

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1, 2, 3, 1, 1, 2, 3, 0)]
    [InlineData(1, 2, 3, 1, 1, 2, 2, 1)]
    [InlineData(1, 2, 3, 1, 1, 1, 3, 1)]
    [InlineData(1, 2, 3, 1, 0, 2, 3, 1)]
    public void NotEqualOp_ShouldBeTrue_WhenNotEqual(double x1, double y1, double z1, double w1,
        double x2, double y2, double z2, double w2)
    {
        // Arrange
        Models.Tuple t1 = new(x1, y1, z1, w1);
        Models.Tuple t2 = new(x2, y2, z2, w2);

        // Act
        var result = t1 != t2;

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void NotEqualOp_ShouldBeTrue_WhenOneNull(bool leftIsNull)
    {
        // Arrange
        Models.Tuple t1, t2;
        if (leftIsNull)
        {
            t1 = null!;
            t2 = new(1, 2, 3, 1);
        }
        else
        {
            t1 = new(1, 2, 3, 1);
            t2 = null!;
        }

        // Act
        var result = t1 != t2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void NotEqualOp_ShouldBeFalse_WhenAllSame()
    {
        // Arrange
        Models.Tuple t1 = new(1, 2, 3, 1);
        Models.Tuple t2 = new(1, 2, 3, 1);

        // Act
        var result = t1 != t2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void NotEqualOp_ShouldBeFalse_WhenBothNull()
    {
        // Arrange
        Models.Tuple t1 = null!;
        Models.Tuple t2 = null!;

        // Act
        var result = t1 != t2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetHashCode_ShouldBeEqual_WhenSameProperties()
    {
        // Arrange
        Models.Tuple t1 = new(1, 2, 3, 1);
        Models.Tuple t2 = new(1, 2, 3, 1);

        // Act
        var t1HashCode = t1.GetHashCode();
        var t2HashCode = t2.GetHashCode();

        // Assert
        Assert.True(t1HashCode == t2HashCode);
    }

    [Theory]
    [InlineData(1, 2, 3, 1, 1, 2, 3, 0)]
    [InlineData(1, 2, 3, 1, 1, 2, 2, 1)]
    [InlineData(1, 2, 3, 1, 1, 1, 3, 1)]
    [InlineData(1, 2, 3, 1, 0, 2, 3, 1)]
    public void GetHashCode_ShouldNotBeEqual_WhenNotAllSame(double x1, double y1, double z1, double w1,
        double x2, double y2, double z2, double w2)
    {
        // Arrange
        Models.Tuple t1 = new(x1, y1, z1, w1);
        Models.Tuple t2 = new(x2, y2, z2, w2);

        // Act
        var t1HashCode = t1.GetHashCode();
        var t2HashCode = t2.GetHashCode();

        // Assert
        Assert.True(t1HashCode != t2HashCode);
    }
    #endregion
}
