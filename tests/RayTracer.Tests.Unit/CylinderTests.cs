using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Shapes;
using Xunit;

namespace RayTracer.Tests.Unit;

public class CylinderTests
{
	[Fact]
	public void Constructor_ShouldSetCorrectMinimumAndMaximum_WhenNoneGiven()
	{
		// Arrange
		const double expectedMinimum = double.NegativeInfinity;
		const double expectedMaximum = double.PositiveInfinity;

		// Act
		Cylinder cylinder = new Cylinder();

		// Assert
		Assert.Equal(expectedMinimum, cylinder.Minimum);
		Assert.Equal(expectedMaximum, cylinder.Maximum);
	}

	[Fact]
	public void Closed_ShouldBeFalse_WhenCylinderInitialized()
	{
		// Arrange
		// Act
		Cylinder cylinder = new();

		// Assert
		Assert.False(cylinder.Closed);
	}

	[Theory]
	[MemberData(nameof(IntersectMissData))]
	public void Intersect_ShouldReturnEmpty_WhenRayMissesCylinder(Point origin, Vector direction)
	{
		// Arrange
		Cylinder cylinder = new();
		Ray ray = new(origin, direction);

		// Act
		var result = cylinder.Intersect(ray);

		// Assert
		Assert.Empty(result);
	}
	public static IEnumerable<object[]> IntersectMissData =>
		new List<object[]>
		{
			new object[] { new Point(1, 0, 0), new Vector(0, 1, 0) },
			new object[] { new Point(0, 0, 0), new Vector(0, 1, 0) },
			new object[] { new Point(0, 0, -5), new Vector(1, 1, 1) }
		};

	[Theory]
	[MemberData(nameof(IntersectHitData))]
	public void Intersect_ShouldBeCorrect_WhenRayHits(Point origin, Vector direction,
		double expectedT1, double expectedT2)
	{
		// Arrange
		Cylinder cylinder = new();
		direction = direction.Normalize();
		Ray ray = new(origin, direction);
		const int expectedLength = 2;

		// Act
		var result = cylinder.Intersect(ray);

		// Assert
		Assert.Equal(expectedLength, result.Length);
		Assert.True(expectedT1.IsEqualTo(result[0].T));
		Assert.True(expectedT2.IsEqualTo(result[1].T));
	}
	public static IEnumerable<object[]> IntersectHitData =>
		new List<object[]>
		{
			new object[] { new Point(1, 0, -5), new Vector(0, 0, 1), 5.0, 5.0 },
			new object[] { new Point(0, 0, -5), new Vector(0, 0, 1), 4.0, 6.0 },
			new object[] { new Point(0.5, 0, -5), new Vector(0.1, 1, 1), 6.80798, 7.08872 }
		};

	[Theory]
	[MemberData(nameof(IntersectTruncatedData))]
	public void Intersect_ShouldBeCorrect_WhenCylinderTruncated(Point origin, Vector direction,
		int expectedIntersectionCount)
	{
		// Arrange
		Cylinder cylinder = new()
		{
			Minimum = 1,
			Maximum = 2
		};
		direction = direction.Normalize();
		Ray ray = new(origin, direction);

		// Act
		var actual = cylinder.Intersect(ray).Length;

		// Assert
		Assert.Equal(expectedIntersectionCount, actual);
	}
	public static IEnumerable<object[]> IntersectTruncatedData =>
		new List<object[]>
		{
			new object[] { new Point(0, 1.5, 0), new Vector(0.1, 1, 0), 0 },
			new object[] { new Point(0, 3, -5), new Vector(0, 0, 1), 0 },
			new object[] { new Point(0, 0, -5), new Vector(0, 0, 1), 0 },
			new object[] { new Point(0, 2, -5), new Vector(0, 0, 1), 0 },
			new object[] { new Point(0, 1, -5), new Vector(0, 0, 1), 0 },
			new object[] { new Point(0, 1.5, -2), new Vector(0, 0, 1), 2 },
		};

	[Theory]
	[MemberData(nameof(IntersectCappedData))]
	public void Intersect_ShouldBeCorrect_WhenCylinderCapped(Point origin, Vector direction,
		int expectedIntersectionCount)
	{
		// Arrange
		Cylinder cylinder = new()
		{
			Minimum = 1,
			Maximum = 2,
			Closed = true
		};
		direction = direction.Normalize();
		Ray ray = new(origin, direction);

		// Act
		var actual = cylinder.Intersect(ray).Length;

		// Assert
		Assert.Equal(expectedIntersectionCount, actual);
	}
	public static IEnumerable<object[]> IntersectCappedData =>
		new List<object[]>
		{
			new object[] { new Point(0, 3, 0), new Vector(0, -1, 0), 2 },
			new object[] { new Point(0, 3, -2), new Vector(0, -1, 2), 2 },
			new object[] { new Point(0, 4, -2), new Vector(0, -1, 1), 2 },
			new object[] { new Point(0, 0, -2), new Vector(0, 1, 2), 2 },
			new object[] { new Point(0, -1, -2), new Vector(0, 1, 1), 2 },
		};

	[Theory]
	[MemberData(nameof(NormalData))]
	public void Normal_ShouldComputeCorrectly(Point point, Vector expected)
	{
		// Arrange
		Cylinder cylinder = new();

		// Act
		var actual = cylinder.Normal(point);

		// Assert
		Assert.Equal(expected, actual);
	}
	public static IEnumerable<object[]> NormalData =>
		new List<object[]>
		{
			new object[] { new Point(1, 0, 0), new Vector(1, 0, 0) },
			new object[] { new Point(0, 5, -1), new Vector(0, 0, -1) },
			new object[] { new Point(0, -2, 1), new Vector(0, 0, 1) },
			new object[] { new Point(-1, 1, 0), new Vector(-1, 0, 0) }
		};


	[Theory]
	[MemberData(nameof(NormalCappedData))]
	public void Normal_ShouldComputeCorrectly_WhenCylinderCapped(Point point, Vector expected)
	{
		// Arrange
		Cylinder cylinder = new()
		{
			Minimum = 1,
			Maximum = 2,
			Closed = true
		};

		// Act
		var actual = cylinder.Normal(point);

		// Assert
		Assert.Equal(expected, actual);
	}
	public static IEnumerable<object[]> NormalCappedData =>
		new List<object[]>
		{
			new object[] { new Point(0, 1, 0), new Vector(0, -1, 0) },
			new object[] { new Point(0.5, 1, 0), new Vector(0, -1, 0) },
			new object[] { new Point(0, 1, 0.5), new Vector(0, -1, 0) },
			new object[] { new Point(0, 2, 0), new Vector(0, 1, 0) },
			new object[] { new Point(0.5, 2, 0), new Vector(0, 1, 0) },
			new object[] { new Point(0, 2, 0.5), new Vector(0, 1, 0) }
		};

	[Fact]
	public void BoundsOf_ShouldBeCorrect_WhenCylinderUnbound()
	{
		// Arrange
		Cylinder cylinder = new();
		Point expectedMinimum = new(-1, double.NegativeInfinity, -1);
		Point expectedMaximum = new(1, double.PositiveInfinity, 1);

		// Act
		var result = cylinder.BoundsOf();

		// Assert
		Assert.Equal(expectedMinimum, result.Minimum);
		Assert.Equal(expectedMaximum, result.Maximum);
	}

	[Fact]
	public void BoundsOf_ShouldBeCorrect_WhenCylinderBound()
	{
		// Arrange
		Cylinder cylinder = new()
		{
			Minimum = -5,
			Maximum = 3
		};
		Point expectedMinimum = new(-1, -5, -1);
		Point expectedMaximum = new(1, 3, 1);

		// Act
		var result = cylinder.BoundsOf();

		// Assert
		Assert.Equal(expectedMinimum, result.Minimum);
		Assert.Equal(expectedMaximum, result.Maximum);
	}
}
