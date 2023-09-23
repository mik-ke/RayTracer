using RayTracer.Models;
using Xunit;
namespace RayTracer.Tests.Unit;

public class IntersectionsTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		Intersections i = new Intersections();

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
	public void Length_ShouldReturnCorrectValue()
	{
		// Arrange
		Sphere sphere = new();
		Intersection i1 = new(1, sphere);
		Intersection i2 = new(2, sphere);
		const int expectedCount = 2;
		Intersections intersections = new(i1, i2);

		// Act
		int actual = intersections.Length;

		// Assert
		Assert.Equal(expectedCount, actual);
	}

	[Fact]
	public void Hit_ShouldReturnCorrectIntersection_WhenAllTsArePositive()
	{
		// Arrange
		Sphere sphere = new();
		Intersection expectedHit = new(1, sphere);
		Intersection intersection = new(2, sphere);
		Intersections intersections = new(intersection, expectedHit);

		// Act
		var actual = intersections.Hit();

		// Assert
		Assert.Equal(expectedHit, actual);
	}

	[Fact]
	public void Hit_ShouldReturnCorrectIntersection_WhenSomeTsAreNegative()
	{
		// Arrange
		Sphere sphere = new();
		Intersection expectedHit = new(1, sphere);
		Intersection intersection = new(-1, sphere);
		Intersections intersections = new(expectedHit, intersection);

		// Act
		var actual = intersections.Hit();

		// Assert
		Assert.Equal(expectedHit, actual);
	}

	[Fact]
	public void Hit_ShouldReturnNull_WhenAllTsAreNegative()
	{
		// Arrange
		Sphere sphere = new();
		Intersection intersection1 = new(-2, sphere);
		Intersection intersection2 = new(-1, sphere);
		Intersections intersections = new(intersection2, intersection1);

		// Act
		var result = intersections.Hit();

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void Hit_ShouldReturnLowestNonNegativeTIntersection()
	{
		// Arrange
		Sphere sphere = new();
		Intersection intersection1 = new(5, sphere);
		Intersection intersection2 = new(7, sphere);
		Intersection intersection3 = new(-3, sphere);
		Intersection expectedHit = new(2, sphere);
		Intersections intersections = new(intersection1, intersection2,
			intersection3, expectedHit);

		// Act
		var actual = intersections.Hit();

		// Assert
		Assert.Equal(expectedHit, actual);
	}
}
