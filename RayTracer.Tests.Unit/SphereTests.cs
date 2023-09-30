using RayTracer.Shapes;
using RayTracer.Models;

namespace RayTracer.Tests.Unit;

public class SphereTests
{
	[Fact]
	public void Sphere_ShouldBeShape()
	{
		// Arrange
		// Act
		Sphere sphere = new();

		// Assert
		Assert.IsAssignableFrom<Shape>(sphere);
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
        Sphere sphere = new()
        {
            Transform = Matrix.Scaling(2, 2, 2)
        };
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
        Sphere sphere = new()
        {
            Transform = Matrix.Translation(5, 0, 0)
        };

        // Act
        var intersections = sphere.Intersect(ray);

		// Assert
		Assert.Empty(intersections);
	}

	[Theory]
	[MemberData(nameof(NormalData))]
	public void Normal_ShouldWork_WhenPointGiven(Point point, Vector expected)
	{
		// Arrange
		Sphere sphere = new();

		// Act
		var actual = sphere.Normal(point);

		// Assert
		Assert.Equal(expected, actual);
	}
	public static IEnumerable<object[]> NormalData =>
		new List<object[]>
		{
			new object[] { new Point(1, 0, 0), new Vector(1, 0, 0) },
			new object[] { new Point(0, 1, 0), new Vector(0, 1, 0) },
			new object[] { new Point(0, 0, 1), new Vector(0, 0, 1) },
			new object[] { new Point(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3), new Vector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3) }
		};

	[Fact]
	public void Normal_ShouldBeNormalizedVector()
	{
		// Arrange
		Sphere sphere = new();
		Point point = new(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3);
		Vector normal = sphere.Normal(point);

		// Act
		var normalized = normal.Normalize();

		// Assert
		Assert.Equal(normal, normalized);
	}
}
