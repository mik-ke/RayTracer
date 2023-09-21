using RayTracer.Models;
using Xunit;
namespace RayTracer.Tests.Unit;

public class SphereTests
{
	[Theory]
	[MemberData(nameof(IntersectData))]
	public void Intersect_ShouldReturnCorrectIntersections_WhenCalledWithRay(Ray ray, double expected, double expected2)
	{
		// Arrange
		Sphere sphere = new();

		// Act
		var actualIntersections = sphere.Intersect(ray);

		// Assert
		Assert.Equal(expected, actualIntersections[0]);
		Assert.Equal(expected2, actualIntersections[1]);
	}
	public static IEnumerable<object[]> IntersectData =
		new List<object[]>
		{
			new object[] { new Ray(new Point(0, 0, -5), new Vector(0, 0, 1)), 4.0, 6.0 },
			new object[] { new Ray(new Point(0, 1, -5), new Vector(0, 0, 1)), 5.0, 5.0 },
		};

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
}
