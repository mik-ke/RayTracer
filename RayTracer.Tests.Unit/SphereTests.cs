using RayTracer.Models;
using Xunit;

namespace RayTracer.Tests.Unit;

public class SphereTests
{
	[Fact]
	public void Transform_ShouldBeIdentityMatrix_WhenInitialized()
	{
		// Arrange
		Matrix expected = Matrix.Identity(4);

		// Act
		Sphere sphere = new();

		// Assert
		Assert.Equal(expected, sphere.Transform);
	}

	[Fact]
	public void Transform_ShouldBeSet()
	{
		// Arrange
		Matrix expected = Matrix.Translation(2, 3, 4);
		Sphere sphere = new();

		// Act
		sphere.Transform = expected;

		// Assert
		Assert.Equal(expected, sphere.Transform);
	}

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
		Assert.Equal(expected0, actualIntersections[0].T);
		Assert.Equal(expected1, actualIntersections[1].T);
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
		Assert.Equal(expected, actualIntersections[0].T);
		Assert.Equal(expected, actualIntersections[1].T);
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
		Assert.Equal(expected0, intersections[0].T);
		Assert.Equal(expected1, intersections[1].T);
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
		Assert.Equal(expected0, intersections[0].T);
		Assert.Equal(expected1, intersections[1].T);
	}

	[Fact]
	public void Intersect_ShouldSetCorrectObjectOnIntersection()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
		Sphere sphere = new();

		// Act
		var intersections = sphere.Intersect(ray);

		// Assert
		Assert.Equal(sphere, intersections[0].Object);
		Assert.Equal(sphere, intersections[1].Object);
	}

	[Fact]
	public void Intersect_ShouldWork_WhenTransformSetToScalingMatrix()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
		Sphere sphere = new();
		sphere.Transform = Matrix.Scaling(2, 2, 2);
		const int expectedLength = 2;
		const double expected0 = 3.0;
		const double expected1 = 7.0;

		// Act
		var intersections = sphere.Intersect(ray);

		// Assert
		Assert.Equal(expectedLength, intersections.Length);
		Assert.Equal(expected0, intersections[0].T);
		Assert.Equal(expected1, intersections[1].T);
	}

	[Fact]
	public void Intersect_ShouldWork_WhenTransformSetToTranslationMatrix()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
		Sphere sphere = new();
		sphere.Transform = Matrix.Translation(5, 0, 0);

		// Act
		var intersections = sphere.Intersect(ray);

		// Assert
		Assert.Empty(intersections);
	}
}
