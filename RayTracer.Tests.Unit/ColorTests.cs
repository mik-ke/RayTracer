using RayTracer.Models;
using Xunit;

namespace RayTracer.Tests.Unit;

public class ColorTests
{
    [Fact]
    public void Constructor_ShouldInitializeCorrectProperties_WhenCreated()
    {
        // Arrange
        const double r = -0.5;
        const double g = 0.4;
        const double b = 1.7;

        // Act
        Color color = new(r, g, b);

        // Assert
        Assert.Equal(r, color.R);
        Assert.Equal(g, color.G);
        Assert.Equal(b, color.B);
    }

    [Fact]
    public void Black_ShouldBeCorrect()
    {
        // Arrange
        Color expected = new(0, 0, 0);

        // Act
        var actual = Color.Black;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void White_ShouldBeCorrect()
    {
        // Arrange
        Color expected = new(1, 1, 1);

        // Act
        var actual = Color.White;

        // Assert
        Assert.Equal(expected, actual);
    }

    #region arithmetic operations
    [Fact]
    public void Add_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Color leftColor = new(0.9, 0.6, 0.75);
        Color rightColor = new(0.7, 0.1, 0.25);
        Color expected = new(1.6, 0.7, 1.0);

        // Act
        Color actual = leftColor.Add(rightColor);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void PlusOperator_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Color leftColor = new(0.9, 0.6, 0.75);
        Color rightColor = new(0.7, 0.1, 0.25);
        Color expected = new(1.6, 0.7, 1.0);

        // Act
        Color actual = leftColor + rightColor;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Subtract_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Color leftColor = new(0.9, 0.6, 0.75);
        Color rightColor = new(0.7, 0.1, 0.25);
        Color expected = new(0.2, 0.5, 0.5);

        // Act
        Color actual = leftColor.Subtract(rightColor);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MinusOperator_ShouldBeCorrect_WhenCalled()
    {
        // Arrange
        Color leftColor = new(0.9, 0.6, 0.75);
        Color rightColor = new(0.7, 0.1, 0.25);
        Color expected = new(0.2, 0.5, 0.5);

        // Act
        Color actual = leftColor - rightColor;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Multiply_ShouldBeCorrect_WhenCalledWithScalar()
    {
        // Arrange
        Color color = new(0.2, 0.3, 0.4);
        const double scalar = 2;
        Color expected = new(0.4, 0.6, 0.8);

        // Act
        Color actual = color.Multiply(scalar);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Multiply_ShouldBeCorrect_WhenCalledWithColor()
    {
        // Arrange
        Color leftColor = new(1, 0.2, 0.4);
        Color rightColor = new(0.9, 1, 0.1);
        Color expected = new(0.9, 0.2, 0.04);

        // Act
        Color actual = leftColor.Multiply(rightColor);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MultiplyOperator_ShouldBeCorrect_WhenCalledWithScalar()
    {
        // Arrange
        Color color = new(0.2, 0.3, 0.4);
        const double scalar = 2;
        Color expected = new(0.4, 0.6, 0.8);

        // Act
        Color actual = color * scalar;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MultiplyOperator_ShouldBeCorrect_WhenCalledWithColor()
    {
        // Arrange
        Color leftColor = new(1, 0.2, 0.4);
        Color rightColor = new(0.9, 1, 0.1);
        Color expected = new(0.9, 0.2, 0.04);

        // Act
        Color actual = leftColor * rightColor;

        // Assert
        Assert.Equal(expected, actual);
    }
    #endregion

    #region equality
    [Fact]
    public void Equals_ShouldBeTrue_WhenAllSame()
    {
        // Arrange
        Color c1 = new(0, 1, 0);
        Color c2 = new(0, 1, 0);

        // Act
        var result = c1 == c2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ShouldBeTrue_WhenBothNull()
    {
        // Arrange
        Color c1 = null!;
        Color c2 = null!;

        // Act
        var result = c1 == c2;

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Equals_ShouldBeFalse_WhenOnlyOneNull(bool leftIsNull)
    {
        // Arrange
        Color c1, c2;
        if (leftIsNull)
        {
            c1 = null!;
            c2 = new Color(0, 1, 0);
        }
        else
        {
            c1 = new Color(0, 1, 0);
            c2 = null!;
        }

        // Act
        var result = c1 == c2;

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(0.1, 0.2, 0.3, 0.1, 0.2, 0.2)]
    [InlineData(0.1, 0.2, 0.3, 0.1, 0.1, 0.3)]
    [InlineData(0.1, 0.2, 0.3, 0.0, 0.2, 0.3)]
    public void Equals_ShouldBeFalse_WhenNotAllSame(double r1, double g1, double b1,
        double r2, double g2, double b2)
    {
        // Arrange
        Color c1 = new(r1, g1, b1);
        Color c2 = new(r2, g2, b2);

        // Act
        var result = c1 == c2;

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1, 2, 3, 1, 2, 2)]
    [InlineData(1, 2, 3, 1, 1, 3)]
    [InlineData(1, 2, 3, 0, 2, 3)]
    public void NotEqualOp_ShouldBeTrue_WhenNotEqual(double r1, double g1, double b1,
        double r2, double g2, double b2)
    {
        // Arrange
        Color c1 = new(r1, g1, b1);
        Color c2 = new(r2, g2, b2);

        // Act
        var result = c1 != c2;

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void NotEqualOp_ShouldBeTrue_WhenOneNull(bool leftIsNull)
    {
        // Arrange
        Color c1, c2;
        if (leftIsNull)
        {
            c1 = null!;
            c2 = new Color(0.1, 0.2, 0.3);
        }
        else
        {
            c1 = new Color(0.1, 0.2, 0.3);
            c2 = null!;
        }

        // Act
        var result = c1 != c2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void NotEqualOp_ShouldBeFalse_WhenAllSame()
    {
        // Arrange
        Color c1 = new(0.1, 0.2, 0.3);
        Color c2 = new(0.1, 0.2, 0.3);

        // Act
        var result = c1 != c2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void NotEqualOp_ShouldBeFalse_WhenBothNull()
    {
        // Arrange
        Color c1 = null!;
        Color c2 = null!;

        // Act
        var result = c1 != c2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetHashCode_ShouldBeEqual_WhenSameProperties()
    {
        // Arrange
        Color c1 = new(0.1, 0.2, 0.3);
        Color c2 = new(0.1, 0.2, 0.3);

        // Act
        var c1HashCode = c1.GetHashCode();
        var c2HashCode = c2.GetHashCode();

        // Assert
        Assert.Equal(c1HashCode, c2HashCode);
    }

    [Theory]
    [InlineData(1, 2, 3, 1, 2, 2)]
    [InlineData(1, 2, 3, 1, 1, 3)]
    [InlineData(1, 2, 3, 0, 2, 3)]
    public void GetHashCode_ShouldNotBeEqual_WhenNotAllSame(double r1, double g1, double b1,
        double r2, double g2, double b2)
    {
        // Arrange
        Color c1 = new(r1, g1, b1);
        Color c2 = new(r2, g2, b2);

        // Act
        var c1HashCode = c1.GetHashCode();
        var c2HashCode = c2.GetHashCode();

        // Assert
        Assert.NotEqual(c1HashCode, c2HashCode);
    }
    #endregion
}
