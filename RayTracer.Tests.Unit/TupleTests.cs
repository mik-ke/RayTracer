using RayTracer.Models;

namespace RayTracer.Tests.Unit;

public class TupleTests
{
    #region equality
    [Fact]
    public void Equals_ShouldBeTrue_WhenAllSame()
    {
        // Arrange
        Models.Tuple p1 = new Point(1, 2, 3);
        Models.Tuple p2 = new Point(1, 2, 3);

        // Act
        var result = p1 == p2;

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
            t2 = new Point(1, 2, 3);
        }
        else
        {
            t1 = new Point(1, 2, 3);
            t2 = null!;
        }

        // Act
        var result = t1 == t2;

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1, 2, 3, 1, 2, 2)]
    [InlineData(1, 2, 3, 1, 1, 3)]
    [InlineData(1, 2, 3, 0, 2, 3)]
    public void Equals_ShouldBeFalse_WhenNotAllSame(double x1, double y1, double z1,
        double x2, double y2, double z2)
    {
        // Arrange
        Models.Tuple t1 = new Point(x1, y1, z1);
        Models.Tuple t2 = new Point(x2, y2, z2);

        // Act
        var result = t1 == t2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_ShouldBeFalse_WhenComparingVectorAndPoint()
    {
        // Arrange
        Models.Tuple vector = new Vector(1, 2, 3);
        Models.Tuple point = new Point(1, 2, 3);

        // Act
        var result = vector == point;

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1, 2, 3, 1, 2, 2)]
    [InlineData(1, 2, 3, 1, 1, 3)]
    [InlineData(1, 2, 3, 0, 2, 3)]
    public void NotEqualOp_ShouldBeTrue_WhenNotEqual(double x1, double y1, double z1,
        double x2, double y2, double z2)
    {
        // Arrange
        Models.Tuple t1 = new Point(x1, y1, z1);
        Models.Tuple t2 = new Point(x2, y2, z2);

        // Act
        var result = t1 != t2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void NotEqualOp_ShouldBeTrue_WhenComparingVectorAndPoint()
    {
        // Arrange
        Models.Tuple vector = new Vector(1, 2, 3);
        Models.Tuple point = new Point(1, 2, 3);

        // Act
        var result = vector != point;

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
            t2 = new Point(1, 2, 3);
        }
        else
        {
            t1 = new Point(1, 2, 3);
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
        Models.Tuple t1 = new Point(1, 2, 3);
        Models.Tuple t2 = new Point(1, 2, 3);

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
        Models.Tuple t1 = new Point(1, 2, 3);
        Models.Tuple t2 = new Point(1, 2, 3);

        // Act
        var t1HashCode = t1.GetHashCode();
        var t2HashCode = t2.GetHashCode();

        // Assert
        Assert.Equal(t1HashCode, t2HashCode);
    }

    [Theory]
    [InlineData(1, 2, 3, 1, 2, 2)]
    [InlineData(1, 2, 3, 1, 1, 3)]
    [InlineData(1, 2, 3, 0, 2, 3)]
    public void GetHashCode_ShouldNotBeEqual_WhenNotAllSame(double x1, double y1, double z1,
        double x2, double y2, double z2)
    {
        // Arrange
        Models.Tuple t1 = new Point(x1, y1, z1);
        Models.Tuple t2 = new Point(x2, y2, z2);

        // Act
        var t1HashCode = t1.GetHashCode();
        var t2HashCode = t2.GetHashCode();

        // Assert
        Assert.NotEqual(t1HashCode, t2HashCode);
    }

    [Fact]
    public void GetHashCode_ShouldNotBeEqual_WhenComparingVectorAndPoint()
    {
        // Arrange
        Models.Tuple t1 = new Point(1, 2, 3);
        Models.Tuple t2 = new Vector(1, 2, 3);

        // Act
        var t1HashCode = t1.GetHashCode();
        var t2HashCode = t2.GetHashCode();

        // Assert
        Assert.NotEqual(t1HashCode, t2HashCode);
    }
    #endregion
}
