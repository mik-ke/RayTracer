using RayTracer.Models;
using RayTracer.Patterns;

namespace RayTracer.Tests.Unit;

public class GradientPatternTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		// Arrange
		// Act
		GradientPattern pattern = new(Color.White, Color.Black);

		// Assert
		Assert.Equal(Color.White, pattern.A);
		Assert.Equal(Color.Black, pattern.B);
	}

	[Theory]
	[InlineData(0.0,  1.0)]
	[InlineData(0.25, 0.75)]
	[InlineData(0.5,  0.5)]
	[InlineData(0.75, 0.25)]
	public void PatternAt_ShouldLinearlyInterpolate(double pointX, double expectedColorComponent)
	{
		// Arrange
		GradientPattern pattern = new(Color.White, Color.Black);
		Point point = new(pointX, 0, 0);
		Color expected = new(expectedColorComponent, expectedColorComponent, expectedColorComponent);

		// Act
		var actual = pattern.PatternAt(point);

		// Assert
		Assert.Equal(expected, actual);
	}
}
