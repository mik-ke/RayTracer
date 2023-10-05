using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Shapes;

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
		Computations result = new(intersection, ray);

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
		Computations result = new(intersection, ray);

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
		Computations result = new(intersection, ray);

		// Assert
		Assert.True(result.IsInside);
	}

	[Fact]
	public void OverPoint_ShouldBeInitializedCorrectly()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
		Sphere shape = new()
		{
			Transform = Matrix.Translation(0, 0, 1)
        };
		Intersection intersection = new(5, shape);

		// Act
		Computations computations = new(intersection, ray);

		// Assert
		Assert.True(computations.OverPoint.Z < -DoubleExtensions.EPSILON / 2);
		Assert.True(computations.Point.Z > computations.OverPoint.Z);
	}

	[Fact]
	public void ReflectVector_ShouldBeCalculatedCorrectly()
	{
		// Arrange
		Plane shape = new();
		Ray ray = new(new Point(0, 1, -1), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
		Intersection intersection = new(Math.Sqrt(2), shape);
		Vector expected = new(0, Math.Sqrt(2) / 2, Math.Sqrt(2) / 2);

        // Act
        var actual = new Computations(intersection, ray).ReflectVector;

		// Assert
		Assert.Equal(expected, actual);
	}
}
