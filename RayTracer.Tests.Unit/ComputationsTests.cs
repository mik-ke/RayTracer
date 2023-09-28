using RayTracer.Models;
using Xunit;

namespace RayTracer.Tests.Unit;

public class ComputationsTests
{
	[Fact]
	public void Constructor_ShouldComputeCorrectly_WhenGivenRayAndIntersection()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
		Sphere shape = new();
		Intersection intersection = new(4, shape);
		Point expectedPoint = new(0, 0, -1);
		Vector expectedEye = new(0, 0, -1);
		Vector expectedNormal = new(0, 0, -1);

		// Act
		Computations result = new(ray, intersection);

		// Assert
		Assert.Equal(intersection.T, result.T);
		Assert.Equal(shape, result.Object);
		Assert.Equal(expectedPoint, result.Point);
		Assert.Equal(expectedEye, result.EyeVector);
		Assert.Equal(expectedNormal, result.NormalVector);
	}

	[Fact]
	public void IsInside_ShouldBeFalse_WhenIntersectionOutsideObject()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
		Sphere shape = new();
		Intersection intersection = new(4, shape);

		// Act
		Computations result = new(ray, intersection);

		// Assert
		Assert.False(result.IsInside);
	}

	[Fact]
	public void IsInside_ShouldBeTrue_WhenIntersectionInsideObject()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, 0), new Vector(0, 0, 1));
		Sphere shape = new();
		Intersection intersection = new(1, shape);

		// Act
		Computations result = new(ray, intersection);

		// Assert
		Assert.True(result.IsInside);
	}
}
