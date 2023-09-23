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

	[Fact]
	public void Transform_ShouldWork_WhenApplyingTranslationMatrix()
	{
		// Arrange
		Ray ray = new(new Point(1, 2, 3), new Vector(0, 1, 0));
		Matrix translation = Matrix.Translation(3, 4, 5);
		Point expectedOrigin = new Point(4, 6, 8);
		Vector expectedDirection = new Vector(0, 1, 0);

		// Act
		var result = ray.Transform(translation);

		// Assert
		Assert.Equal(expectedOrigin, result.Origin);
		Assert.Equal(expectedDirection, result.Direction);
	}

	[Fact]
	public void Transform_ShouldWork_WhenApplyingScalingMatrix()
	{
		// Arrange
		Ray ray = new(new Point(1, 2, 3), new Vector(0, 1, 0));
		Matrix scaling = Matrix.Scaling(2, 3, 4);
		Point expectedOrigin = new Point(2, 6, 12);
		Vector expectedDirection = new Vector(0, 3, 0);

		// Act
		var result = ray.Transform(scaling);

		// Assert
		Assert.Equal(expectedOrigin, result.Origin);
		Assert.Equal(expectedDirection, result.Direction);
	}
}
