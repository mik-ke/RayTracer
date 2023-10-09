using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Shapes;
using Xunit;

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
		var direction = new Vector(1, 1, 1).Normalize();
		Ray ray = new(new Point(0, 0, -1), direction);
		const int expectedLength = 1;
		const double expectedT = 0.35355;

		// Act
		var result = cone.Intersect(ray);

		// Assert
		Assert.Equal(expectedLength, result.Length);
		Assert.True(expectedT.IsEqualTo(result[0].T));
	}
}
