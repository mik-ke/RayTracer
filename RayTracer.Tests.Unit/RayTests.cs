using RayTracer.Models;
using Xunit;

namespace RayTracer.Tests.Unit;

public class RayTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		// Arrange
		Point origin = new(1, 2, 3);
		Vector direction = new(4, 5, 6);

		// Act
		Ray ray = new(origin, direction);

		// Assert
		Assert.Equal(origin, ray.Origin);
		Assert.Equal(direction, ray.Direction);
	}

	[Theory]
	[MemberData(nameof(PositionData))]
	public void Position_ShouldReturnCorrectPoint(double t, Point expected)
	{
		// Arrange
		Point point = new(2, 3, 4);
		Vector vector = new(1, 0, 0);
		Ray ray = new(point, vector);

		// Act
		var actual = ray.Position(t);

		// Assert
		Assert.Equal(expected, actual);
	}
	public static IEnumerable<object[]> PositionData =
		new List<object[]>
		{
			new object[] { 0, new Point(2, 3, 4) },
			new object[] { 1, new Point(3, 3, 4) },
			new object[] { -1, new Point(1, 3, 4) },
			new object[] { 2.5, new Point(4.5, 3, 4) },
		};
}
