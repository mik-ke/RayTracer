using RayTracer.Models;
using RayTracer.Shapes;

namespace RayTracer.Tests.Unit;

public class SmoothTriangleTests
{
    private static SmoothTriangle GetTestSmoothTriangle() =>
        new(
            new Point(0, 1, 0),
            new Point(-1, 0, 0),
            new Point(1, 0, 0),
            new Vector(0, 1, 0),
            new Vector(-1, 0, 0),
            new Vector(1, 0, 0));

    [Fact]
    public void Constructor_ShouldInitializePointsAndNormals()
    {
        // Arrange
        Point expectedPoint1 = new(0, 1, 0);
        Point expectedPoint2 = new(-1, 0, 0);
        Point expectedPoint3 = new(1, 0, 0);
        Vector expectedNormal1 = new(0, 1, 0);
        Vector expectedNormal2 = new(-1, 0, 0);
        Vector expectedNormal3 = new(1, 0, 0);

        // Act
        SmoothTriangle smoothTriangle = GetTestSmoothTriangle();

        // Assert
        Assert.Equal(expectedPoint1, smoothTriangle.Point1);
        Assert.Equal(expectedPoint2, smoothTriangle.Point2);
        Assert.Equal(expectedPoint3, smoothTriangle.Point3);
        Assert.Equal(expectedNormal1, smoothTriangle.Normal1);
        Assert.Equal(expectedNormal2, smoothTriangle.Normal2);
        Assert.Equal(expectedNormal3, smoothTriangle.Normal3);
    }
}
