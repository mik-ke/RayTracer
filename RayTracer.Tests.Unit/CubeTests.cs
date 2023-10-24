using RayTracer.Models;
using RayTracer.Shapes;
using Xunit;

namespace RayTracer.Tests.Unit;

public class CubeTests
{
	[Theory]
	[MemberData(nameof(IntersectData))]
	public void Intersect_ShouldBeCorrect_WhenRayFromAnyFace(Point origin, Vector direction,
		double expectedT1, double expectedT2)
	{
		// Arrange
		Cube cube = new();
		Ray ray = new(origin, direction);

		// Act
		var result = cube.Intersect(ray);

		// Assert
		Assert.Equal(expectedT1, result[0].T);
		Assert.Equal(expectedT2, result[1].T);
	}
	public static IEnumerable<object[]> IntersectData =>
		new List<object[]>
		{
			new object[] { new Point(5, 0.5, 0), new Vector(-1, 0, 0), 4.0, 6.0 },
			new object[] { new Point(-5, 0.5, 0), new Vector(1, 0, 0), 4.0, 6.0 },
			new object[] { new Point(0.5, 5, 0), new Vector(0, -1, 0), 4.0, 6.0 },
			new object[] { new Point(0.5, -5, 0), new Vector(0, 1, 0), 4.0, 6.0 },
			new object[] { new Point(0.5, 0, 5), new Vector(0, 0, -1), 4.0, 6.0 },
			new object[] { new Point(0.5, 0, -5), new Vector(0, 0, 1), 4.0, 6.0 },
			new object[] { new Point(0, 0.5, 0), new Vector(0, 0, 1), -1.0, 1.0 },
		};

	[Theory]
	[MemberData(nameof(IntersectMissData))]
	public void Intersect_ShouldReturnEmpty_WhenRayMissesCube(Point origin, Vector destination)
	{
		// Arrange
		Cube cube = new();
		Ray ray = new(origin, destination);

		// Act
		var result = cube.Intersect(ray);

		// Assert
		Assert.Empty(result);
	}
	public static IEnumerable<object[]> IntersectMissData =>
		new List<object[]>
		{
			new object[] { new Point(-2, 0, 0), new Vector(0.2673, 0.5345, 0.8018) },
			new object[] { new Point(0, -2, 0), new Vector(0.8018, 0.2673, 0.5345) },
			new object[] { new Point(0, 0, -2), new Vector(0.5345, 0.8018, 0.2673) },
			new object[] { new Point(2, 0, 2), new Vector(0, 0, 1) },
			new object[] { new Point(0, 2, 2), new Vector(0, -1, 0) },
			new object[] { new Point(2, 2, 0), new Vector(-1, 0, 0) },
		};

	[Theory]
	[MemberData(nameof(NormalData))]
	public void Normal_ShouldBeCorrect_WhenAnyPlane(Point intersectionPoint, Vector expected)
	{
		// Arrange
		Cube cube = new();

		// Act
		var actual = cube.Normal(intersectionPoint);

		// Assert
		Assert.Equal(expected, actual);
	}
	public static IEnumerable<object[]> NormalData =>
		new List<object[]>
		{
			new object[] { new Point(1, 0.5, 0.8), new Vector(1, 0, 0) },
			new object[] { new Point(-1, 0.2, 0.9), new Vector(-1, 0, 0) },
			new object[] { new Point(-0.4, 1, -0.1), new Vector(0, 1, 0) },
			new object[] { new Point(0.3, -1, -0.7), new Vector(0, -1, 0) },
			new object[] { new Point(-0.6, 0.3, 1), new Vector(0, 0, 1) },
			new object[] { new Point(0.4, 0.4, -1), new Vector(0, 0, -1) },
			new object[] { new Point(1, 1, 1), new Vector(1, 0, 0) },
			new object[] { new Point(-1, -1, -1), new Vector(-1, 0, 0) },
		};

	[Fact]
	public void BoundsOf_ShouldBeCorrect()
	{
		// Arrange
		Cube cube = new();
		Point expectedMinimum = new(-1, -1, -1);
		Point expectedMaximum = new(1, 1, 1);

		// Act
		var result = cube.BoundsOf();

		// Assert
		Assert.Equal(expectedMinimum, result.Minimum);
		Assert.Equal(expectedMaximum, result.Maximum);
	}
}
