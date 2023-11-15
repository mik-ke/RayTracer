using RayTracer.Models;
using RayTracer.Patterns;
using RayTracer.Shapes;

namespace RayTracer.Tests.Unit;

public class StripePatternTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		// Arrange
		// Act
		StripePattern pattern = new(Color.White, Color.Black);

		// Assert
		Assert.Equal(Color.White, pattern.A);
		Assert.Equal(Color.Black, pattern.B);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(2)]
	public void PatternAt_ShouldBeConstant_WhenYCoordinateChanges(double yCoordinate)
	{
		// Arrange
		StripePattern pattern = new(Color.White, Color.Black);
		Point point = new(0, yCoordinate, 0);
		Color expected = Color.White;

		// Act
		var actual = pattern.PatternAt(point);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(2)]
	public void PatternAt_ShouldBeConstant_WhenZCoordinateChanges(double zCoordinate)
	{
		// Arrange
		StripePattern pattern = new(Color.White, Color.Black);
		Point point = new(0, 0, zCoordinate);
		Color expected = Color.White;

		// Act
		var actual = pattern.PatternAt(point);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[MemberData(nameof(StripeAtData))]
	public void PatternAt_ShouldAlternate_WhenXCoordinateChanges(double xCoordinate, Color expected)
	{
		// Arrange
		StripePattern pattern = new(Color.White, Color.Black);
		Point point = new(xCoordinate, 0, 0);

		// Act
		var actual = pattern.PatternAt(point);

		// Assert
		Assert.Equal(expected, actual);
	}
	public static IEnumerable<object[]> StripeAtData =>
		new List<object[]>
		{
			new object[] { 0.0, Color.White },
			new object[] { 0.9, Color.White },
			new object[] { 1, Color.Black },
			new object[] { -0.1, Color.Black },
			new object[] { -1, Color.Black },
			new object[] { -1.1, Color.White },
		};
}
