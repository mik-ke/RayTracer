using RayTracer.Models;
namespace RayTracer.Tests.Unit;

public class IntersectionsTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		// Arrange
		Sphere sphere = new();
		const double t1 = 1.0;
		const double t2 = 2.0;
		Intersection i1 = new(t1, sphere);
		Intersection i2 = new(t2, sphere);

		// Act
		Intersections intersections = new(i1, i2);

		// Assert
		Assert.Equal(t1, intersections[0].T);
		Assert.Equal(t2, intersections[1].T);
	}

	[Fact]
	public void Count_ShouldReturnCorrectValue()
	{
		// Arrange
		Sphere sphere = new();
		Intersection i1 = new(1, sphere);
		Intersection i2 = new(2, sphere);
		const int expectedCount = 2;

		// Act
		Intersections intersections = new(i1, i2);

		// Assert
		Assert.Equal(expectedCount, intersections.Count);
	}
}
