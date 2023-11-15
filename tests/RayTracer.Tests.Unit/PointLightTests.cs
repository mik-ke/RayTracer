using RayTracer.Models;

namespace RayTracer.Tests.Unit;

public class PointLightTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		// Arrange
		Point position = new(0, 0, 0);
		Color intensity = new(1, 1, 1);

		// Act
		var light = new PointLight(position, intensity);

		// Assert
		Assert.Equal(intensity, light.Intensity);
		Assert.Equal(position, light.Position);
	}
}
