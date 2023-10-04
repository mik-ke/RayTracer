using RayTracer.Models;
using RayTracer.Patterns;

namespace RayTracer.Tests.Unit;

public class CheckersPatternTests
{
	public static IEnumerable<object[]> PatternAtData =>
		new List<object[]> {
            new object[] { 0.0, Color.White },
            new object[] { 0.99, Color.White },
            new object[] { 1.01, Color.Black },
		};

	[Theory]
	[MemberData(nameof(PatternAtData))]
	public void PatternAt_ShouldRepeat_WhenXChanged(double x, Color expected)
	{
		// Arrange
		CheckersPattern pattern = new(Color.White, Color.Black);
		Point point = new(x, 0, 0);

		// Act
		var actual = pattern.PatternAt(point);

		// Assert
		Assert.Equal(expected, actual);
	}


	[Theory]
	[MemberData(nameof(PatternAtData))]
	public void PatternAt_ShouldRepeat_WhenYChanged(double y, Color expected)
	{
		// Arrange
		CheckersPattern pattern = new(Color.White, Color.Black);
		Point point = new(0, y, 0);

		// Act
		var actual = pattern.PatternAt(point);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[MemberData(nameof(PatternAtData))]
	public void PatternAt_ShouldRepeat_WhenZChanged(double z, Color expected)
	{
		// Arrange
		CheckersPattern pattern = new(Color.White, Color.Black);
		Point point = new(0, 0, z);

		// Act
		var actual = pattern.PatternAt(point);

		// Assert
		Assert.Equal(expected, actual);
	}
}
