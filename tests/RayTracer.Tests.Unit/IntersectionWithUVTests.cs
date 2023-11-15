using RayTracer.Models;
using RayTracer.Shapes;

namespace RayTracer.Tests.Unit;

public class IntersectionWithUVTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
        // Arrange
        Triangle triangle = new(new(0, 1, 0), new(-1, 0, 0), new(1, 0, 0));
		const double expectedU = 0.2;
		const double expectedV = 0.4;

		// Act
		IntersectionWithUV intersection = new(3.5, triangle, expectedU, expectedV);

		// Assert
		Assert.Equal(expectedU, intersection.U);
		Assert.Equal(expectedV, intersection.V);
	}
}
