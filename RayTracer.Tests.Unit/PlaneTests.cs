using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Shapes;
using Xunit;

namespace RayTracer.Tests.Unit;

public class PlaneTests
{
	[Theory]
	[MemberData(nameof(NormalData))]
	public void Normal_ShouldBeSame_WhenGivenDifferentPoints(Point point)
	{
		// Arrange
		Plane plane = new();
		Vector expected = new(0, 1, 0);

		// Act
		var actual = plane.Normal(point);

		// Assert
		Assert.Equal(expected, actual);
	}
	public static IEnumerable<object[]> NormalData =>
		new List<object[]>
		{
			new object[] { new Point(0, 0, 0) },
			new object[] { new Point(10, 0, -10) },
			new object[] { new Point(-5, 0, 150) }
		};

	[Fact]
	public void Intersect_ShouldBeEmpty_WhenRayParallelToPlane()
	{
		// Arrange
		Plane plane = new();
		Ray ray = new(new Point(0, 10, 0), new Vector(0, 0, 1));

		// Act
		var result = plane.Intersect(ray);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void Intersect_ShouldBeEmpty_WhenRayCoplanar()
	{
		// Arrange
		Plane plane = new();
		Ray ray = new(new Point(0, 0, 0), new Vector(0, 0, 1));

		// Act
		var result = plane.Intersect(ray);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void Intersect_ShouldBeCorrect_WhenRayAbovePlane()
	{
		// Arrange
		Plane plane = new();
		Ray ray = new(new Point(0, 1, 0), new Vector(0, -1, 0));
		const double expectedT = 1.0;
		Shape expectedObject = plane;

		// Act
		var result = plane.Intersect(ray);

		// Assert
		Assert.True(expectedT.IsEqualTo(result[0].T));
		Assert.Equal(expectedObject, result[0].Object);
	}

	[Fact]
	public void BoundsOf_ShouldBeCorrect()
	{
		// Arrange
		Plane plane = new();
		Point expectedMinimum = new(double.NegativeInfinity, 0, double.NegativeInfinity);
		Point expectedMaximum = new(double.PositiveInfinity, 0, double.PositiveInfinity);

		// Act
		var result = plane.BoundsOf();

		// Assert
		Assert.Equal(expectedMinimum, result.Minimum);
		Assert.Equal(expectedMaximum, result.Maximum);
	}
}
