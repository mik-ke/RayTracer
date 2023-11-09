using RayTracer.Extensions;
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

    [Fact]
    public void Intersect_ShouldReturnUAndVValues()
    {
        // Arrange
        SmoothTriangle smoothTriangle = GetTestSmoothTriangle();
        Ray ray = new(new Point(-0.2, 0.3, -2), new Vector(0, 0, 1));

        // Act
        Intersections result = smoothTriangle.Intersect(ray);

        // Assert
        Assert.True(0.45.IsEqualTo(((IntersectionWithUV)result[0]).U));
        Assert.True(0.25.IsEqualTo(((IntersectionWithUV)result[0]).V));
    }

    [Fact]
    public void Normal_ShouldInterpolateTheNormal()
    {
        // Arrange
        SmoothTriangle smoothTriangle = GetTestSmoothTriangle();
        IntersectionWithUV intersection = new(1, smoothTriangle, 0.45, 0.25);

        // Act
        Vector result = smoothTriangle.Normal(new Point(0, 0, 0), intersection);

        // Assert
        Assert.Equal(new Vector(-0.5547, 0.83205, 0), result);
    }
    
}
