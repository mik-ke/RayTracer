using RayTracer.Models;
using Xunit;
namespace RayTracer.Tests.Unit;

public class SphereTests
{
	[Fact]
	public void Intersect_ShouldReturnCorrectIntersections_WhenRayOriginatesBeforeSphere()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
		Sphere sphere = new();
		const double expected0 = 4.0;
		const double expected1 = 6.0;

		// Act
		var actualIntersections = sphere.Intersect(ray);

		// Assert
		Assert.Equal(expected0, actualIntersections[0]);
		Assert.Equal(expected1, actualIntersections[1]);
	}

	[Fact]
	public void Intersect_ShouldReturnCorrectIntersectionTwice_WhenRayHitsSphereAtTangent()
	{
		// Arrange
		Ray ray = new(new Point(0, 1, -5), new Vector(0, 0, 1));
		Sphere sphere = new();
		const double expected = 5.0;

		// Act
		var actualIntersections = sphere.Intersect(ray);

		// Assert
		Assert.Equal(expected, actualIntersections[0]);
		Assert.Equal(expected, actualIntersections[1]);
	}

	[Fact]
	public void Intersect_ShouldReturnEmptyCollection_WhenCalledWithRayThatDoesntIntersect()
	{
		// Arrange
		Ray ray = new(new Point(0, 2, -5), new Vector(0, 0, 1));
		Sphere sphere = new();

		// Act
		var intersections = sphere.Intersect(ray);

		// Assert
		Assert.Empty(intersections);
	}

	[Fact]
	public void Intersect_ShouldReturnBothPoints_WhenRayOriginatesInsideSphere()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, 0), new Vector(0, 0, 1));
		Sphere sphere = new();
		const double expected0 = -1.0;
		const double expected1 = 1.0;

		// Act
		var intersections = sphere.Intersect(ray);

		// Assert
		Assert.Equal(expected0, intersections[0]);
		Assert.Equal(expected1, intersections[1]);
	}

	[Fact]
	public void Intersect_ShouldReturnBothPoints_WhenRayOriginatesAfterSphere()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, 5), new Vector(0, 0, 1));
		Sphere sphere = new();
		const double expected0 = -6.0;
		const double expected1 = -4.0;

		// Act
		var intersections = sphere.Intersect(ray);

		// Assert
		Assert.Equal(expected0, intersections[0]);
		Assert.Equal(expected1, intersections[1]);
	}
}
