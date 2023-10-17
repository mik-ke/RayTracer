using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Shapes;

namespace RayTracer.Tests.Unit;

public class TriangleTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		// Arrange
		Point point1 = new(0, 1, 0);
		Point point2 = new(-1, 0, 0);
		Point point3 = new(1, 0, 0);
		Vector edge1 = new(-1, -1, 0);
		Vector edge2 = new(1, -1, 0);
		Vector normal = new(0, 0, -1);

		// Act
		Triangle triangle = new(point1, point2, point3);

		// Assert
		Assert.Equal(point1, triangle.Point1);
		Assert.Equal(point2, triangle.Point2);
		Assert.Equal(point3, triangle.Point3);
		Assert.Equal(edge1, triangle.Edge1);
		Assert.Equal(edge2, triangle.Edge2);
		Assert.Equal(normal, triangle.NormalVector);
	}

	[Theory]
	[MemberData(nameof(NormalData))]
	public void Normal_ShouldBePrecomputed_WhenDifferentPointsUsed(Point normalPoint)
	{
		// Arrange
		Triangle triangle = new(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));

		// Act
		var result = triangle.Normal(normalPoint);

		// Assert
		Assert.Equal(triangle.NormalVector, result);
	}
	public static IEnumerable<object[]> NormalData =>
		new List<object[]>
		{
			new object[] { new Point (0, 0.5, 0) },
			new object[] { new Point (-0.5, 0.75, 0) },
			new object[] { new Point (0.5, 0.25, 0) },
		};

	[Fact]
	public void Intersect_ShouldReturnEmpty_WhenRayParallel()
	{
		// Arrange
		Triangle triangle = new(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
		Ray ray = new(new Point(0, -1, -2), new Vector(0, 1, 0));

		// Act
		var result = triangle.Intersect(ray);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void Intersect_ShouldReturnEmpty_WhenRayMissesP1P3Edge()
	{
		// Arrange
		Triangle triangle = new(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
		Ray ray = new(new Point(1, 1, -2), new Vector(0, 0, 1));

		// Act
		var result = triangle.Intersect(ray);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void Intersect_ShouldReturnEmpty_WhenRayMissesP1P2Edge()
	{
		// Arrange
		Triangle triangle = new(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
		Ray ray = new(new Point(-1, 1, -2), new Vector(0, 0, 1));

		// Act
		var result = triangle.Intersect(ray);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void Intersect_ShouldReturnEmpty_WhenRayMissesP2P3Edge()
	{
		// Arrange
		Triangle triangle = new(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
		Ray ray = new(new Point(0, -1, -2), new Vector(0, 0, 1));

		// Act
		var result = triangle.Intersect(ray);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void Intersect_ShouldBeCorrect_WhenRayStrikesTriangle()
	{
		// Arrange
		Triangle triangle = new(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
		Ray ray = new(new Point(0, 0.5, -2), new Vector(0, 0, 1));
		const int expectedCount = 1;
		const double expectedT = 2.0;

		// Act
		var result = triangle.Intersect(ray);

		// Assert
		Assert.Equal(expectedCount, result.Length);
		Assert.True(expectedT.IsEqualTo(result[0].T));
	}
}
