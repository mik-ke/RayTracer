using RayTracer.Models;
using RayTracer.Shapes;

namespace RayTracer.Tests.Unit;

public class TriangleTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		// Arrange
		Point point1 = new(0, 1, 0);
		Point point2 = new(-1, 0, 0);
		Point point3 = new(1, 0, 0);
		Vector edge1 = new(-1, -1, 0);
		Vector edge2 = new(1, -1, 0);
		Vector normal = new(0, 0, -1);

		// Act
		Triangle triangle = new(point1, point2, point3);

		// Assert
		Assert.Equal(point1, triangle.Point1);
		Assert.Equal(point2, triangle.Point2);
		Assert.Equal(point3, triangle.Point3);
		Assert.Equal(edge1, triangle.Edge1);
		Assert.Equal(edge2, triangle.Edge2);
		Assert.Equal(normal, triangle.NormalVector);
	}
}
