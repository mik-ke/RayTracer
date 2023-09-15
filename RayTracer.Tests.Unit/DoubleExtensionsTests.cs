using RayTracer.Extensions;

namespace RayTracer.Tests.Unit;

public class DoubleExtensionsTests
{
    [Fact]
    public void IsEqualTo_ShouldBeTrue_WhenSameNumber()
    {
        // Arrange
        const double a = 1d;
        const double b = 1d;

        // Act
        bool result = a.IsEqualTo(b);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEqualTo_ShouldBeTrue_WhenDifferencLessThanEPSILON()
    {
        // Arrange
        const double a = 1.12345;
        const double b = 1.123441;

        // Act
        bool result = a.IsEqualTo(b);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEqualTo_ShouldBeFalse_WhenDifferentNumbers()
    {
        // Arrange
        const double a = 1;
        const double b = 2;

        // Act
        bool result = a.IsEqualTo(b);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsEqualTo_ShouldBeFalse_WhenDifferenceEqualToEPSILON()
    {
        // Arrange
        const double a = 1.12345;
        const double b = 1.12344;

        // Act
        bool result = a.IsEqualTo(b);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsEqualTo_ShouldBeFalse_WhenDifferenceGreaterThanEPSILON()
    {
        // Arrange
        const double a = 1.12345;
        const double b = 1.123439;

        // Act
        bool result = a.IsEqualTo(b);

        // Assert
        Assert.False(result);
    }
}
