using RayTracer.Models;

namespace RayTracer.Tests.Unit;

public class PointLightTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		// Arrange
		Color intensity = new(1, 1, 1);
		Point position = new(0, 0, 0);

		// Act
		var light = new PointLight(intensity, position);

		// Assert
		Assert.Equal(intensity, light.Intensity);
		Assert.Equal(position, light.Position);
	}
}
