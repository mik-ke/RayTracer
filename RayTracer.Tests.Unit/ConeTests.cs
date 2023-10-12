using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Shapes;

namespace RayTracer.Tests.Unit;

public class ConeTests
{
	[Theory]
	[MemberData(nameof(IntersectHitData))]
	public void Intersect_ShouldBeCorrect_WhenRayHits(Point origin, Vector direction,
		double expectedT1, double expectedT2)
	{
		// Arrange
		Cone cone = new();
		direction = direction.Normalize();
		Ray ray = new(origin, direction);
		const int expectedLength = 2;

		// Act
		var result = cone.Intersect(ray);

		// Assert
		Assert.Equal(expectedLength, result.Length);
		Assert.True(expectedT1.IsEqualTo(result[0].T));
		Assert.True(expectedT2.IsEqualTo(result[1].T));
	}
	public static IEnumerable<object[]> IntersectHitData =>
		new List<object[]>
		{
			new object[] { new Point(0, 0, -5), new Vector(0, 0, 1), 5.0, 5.0 },
			new object[] { new Point(0, 0, -5), new Vector(1, 1, 1), 8.66025, 8.66025 },
			new object[] { new Point(1, 1, -5), new Vector(-0.5, -1, 1), 4.55006, 49.44994 }
		};

	[Fact]
	public void Intersect_ShouldWork_WhenParallelToOneConeHalf()
	{
		// Arrange
		Cone cone = new();
		var direction = new Vector(0, 1, 1).Normalize();
		Ray ray = new(new Point(0, 0, -1), direction);
		const int expectedLength = 1;
		const double expectedT = 0.35355;

		// Act
		var result = cone.Intersect(ray);

		// Assert
		Assert.Equal(expectedLength, result.Length);
		Assert.True(expectedT.IsEqualTo(result[0].T));
	}

	[Theory]
	[MemberData(nameof(IntersectCappedData))]
	public void Intersect_ShouldBeCorrect_WhenConeCapped(Point origin, Vector direction, int expectedIntersectionCount)
	{
		// Arrange
		Cone cone = new()
		{
			Minimum = -0.5,
			Maximum = 0.5,
			Closed = true
		};
		direction = direction.Normalize();
		Ray ray = new(origin, direction);

		// Act
		var result = cone.Intersect(ray);

		// Assert
		Assert.Equal(expectedIntersectionCount, result.Length);
	}
	public static IEnumerable<object[]> IntersectCappedData =>
		new List<object[]>
		{
			new object[] { new Point(0, 0, -5), new Vector(0, 1, 0), 0 },
			new object[] { new Point(0, 0, -0.25), new Vector(0, 1, 1), 2 },
			new object[] { new Point(0, 0, -0.25), new Vector(0, 1, 0), 4 },
		};

	[Theory]
	[MemberData(nameof(NormalData))]
	public void Normal_ShouldComputeCorrectly(Point point, Vector expected)
	{
		// Arrange
		Cone cone = new();

		// Act
		var actual = cone.Normal(point);

		// Assert
		Assert.Equal(expected.Normalize(), actual);
	}
	public static IEnumerable<object[]> NormalData =>
		new List<object[]>
		{
			new object[] { new Point(0, 0, 0), new Vector(0, 0, 0) },
			new object[] { new Point(1, 1, 1), new Vector(1, -Math.Sqrt(2), 1) },
			new object[] { new Point(-1, -1, 0), new Vector(-1, 1, 0) },
		};
}
