using RayTracer.Models;

namespace RayTracer.Tests.Unit;

public class IntersectionTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		// Arrange
		const double t = 3.5;
		Sphere sphere = new();

		// Act
		Intersection intersection = new(t, sphere);

		// Assert
		Assert.Equal(t, intersection.T);
		Assert.Equal(sphere, intersection.Object);
	}
}
