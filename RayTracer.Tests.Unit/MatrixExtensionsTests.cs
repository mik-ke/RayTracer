using RayTracer.Extensions;
using RayTracer.Models;

namespace RayTracer.Tests.Unit;

public class MatrixExtensionsTests
{
    [Fact]
    public void ChainedTransformations_ShouldWork_WhenMultipliedByPoint()
    {
        // Arrange
        Point point = new(1, 0, 1);
        Point expected = new(15, 0, 7);

        // Act
        var actual = ((Matrix)point)
            .RotateX(Math.PI / 2)
            .Scale(5, 5, 5)
            .Translate(10, 5, 7);

        // Assert
        Assert.Equal(actual, expected);
    }
}
