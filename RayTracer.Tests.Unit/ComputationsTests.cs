using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Shapes;
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

	private Sphere TestGlassSphere()
	{
		Sphere sphere = new();
		sphere.Material.Transparency = 1.0;
		sphere.Material.RefractiveIndex = 1.5;
		return sphere;
	}

	[Theory]
	[InlineData(0, 1.0, 1.5)]
	[InlineData(1, 1.5, 2.0)]
	[InlineData(2, 2.0, 2.5)]
	[InlineData(3, 2.5, 2.5)]
	[InlineData(4, 2.5, 1.5)]
	[InlineData(5, 1.5, 1.0)]
	public void N1N2_ShouldBeCorrect_WhenVariousIntersections(int index, double expectedN1, double expectedN2)
	{
		// Arrange
		Sphere glassSphereA = TestGlassSphere();
		glassSphereA.Transform = Matrix.Scaling(2, 2, 2);
		glassSphereA.Material.RefractiveIndex = 1.5;
		Sphere glassSphereB = TestGlassSphere();
		glassSphereA.Transform = Matrix.Translation(0, 0, -0.25);
		glassSphereA.Material.RefractiveIndex = 2.0;
		Sphere glassSphereC = TestGlassSphere();
		glassSphereA.Transform = Matrix.Translation(0, 0, 0.25);
		glassSphereA.Material.RefractiveIndex = 2.5;
		Ray ray = new(new Point(0, 0, -4), new Vector(0, 0, 1));
		Intersections intersections = new Intersections(
			new Intersection(2, glassSphereA),
			new Intersection(2.75, glassSphereB),
			new Intersection(3.25, glassSphereC),
			new Intersection(4.75, glassSphereB),
			new Intersection(5.25, glassSphereC),
			new Intersection(6, glassSphereA));

		// Act
		Computations computations = new(intersections[index], ray, intersections);

		// Assert
		Assert.Equal(expectedN1, computations.N1);
		Assert.Equal(expectedN2, computations.N2);
	}
}
