using RayTracer.Models;
using RayTracer.Shapes;
using Xunit;

namespace RayTracer.Tests.Unit;

public class BoundingBoxTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly_WhenNoMinMaxGiven()
	{
		// Arrange
        Point expectedMinimum = new(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        Point expectedMaximum = new(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

		// Act
		var boundingBox = new BoundingBox();

		// Assert
		Assert.Equal(expectedMinimum, boundingBox.Minimum);
		Assert.Equal(expectedMaximum, boundingBox.Maximum);
	}

	[Fact]
	public void Constructor_ShouldInitializeCorrectly_WhenMinMaxGiven()
	{
		// Arrange
		Point expectedMinimum = new(-1, -2, -3);
		Point expectedMaximum = new(3, 2, 1);

		// Act
		var boundingBox = new BoundingBox(expectedMinimum, expectedMaximum);

		// Assert
		Assert.Equal(expectedMinimum, boundingBox.Minimum);
		Assert.Equal(expectedMaximum, boundingBox.Maximum);
	}

	[Fact]
	public void Add_ShouldAdjustMinMax_WhenPointGiven()
	{
		// Arrange
		BoundingBox boundingBox = new();
		Point pointOne = new(-5, 2, 0);
		Point pointTwo = new(7, 0, -3);
		Point expectedMinimum = new(-5, 0, -3);
		Point expectedMaximum = new(7, 2, 0);

		// Act
		boundingBox.Add(pointOne);
		boundingBox.Add(pointTwo);

		// Assert
		Assert.Equal(expectedMinimum, boundingBox.Minimum);
		Assert.Equal(expectedMaximum, boundingBox.Maximum);
	}

	[Fact]
	public void Add_ShouldAdjustMinMax_WhenOtherBoundingBoxGiven()
	{
		// Arrange
		BoundingBox boxOne = new(minimum: new(-5, -2, 0), new(7, 4, 4));
		BoundingBox boxTwo = new(minimum: new(8, -7, -2), new(14, 2, 8));
		Point expectedMin = new(-5, -7, -2);
		Point expectedMax = new(14, 4, 8);

		// Act
		boxOne.Add(boxTwo);

		// Assert
		Assert.Equal(expectedMin, boxOne.Minimum);
		Assert.Equal(expectedMax, boxOne.Maximum);
	}

	[Theory]
	[InlineData(5, -2, 0)]
	[InlineData(11, 4, 7)]
	[InlineData(8, 1, 3)]
	public void Contains_ShouldBeTrue_WhenPointInBox(double pointX, double pointY, double pointZ)
	{
		// Arrange
		BoundingBox boundingBox = new(minimum: new(5, -2, 0), maximum: new(11, 4, 7));
		Point point = new(pointX, pointY, pointZ);

		// Act
		var result = boundingBox.Contains(point);

		// Assert
		Assert.True(result);
	}

	[Theory]
	[InlineData(3, 0, 3)]
	[InlineData(8, -4, 3)]
	[InlineData(8, 1, -1)]
	[InlineData(13, 1, 3)]
	[InlineData(8, 5, 3)]
	[InlineData(8, 1, 8)]
	public void Contains_ShouldBeFalse_WhenPointOutsideBox(double pointX, double pointY, double pointZ)
	{
		// Arrange
		BoundingBox boundingBox = new(minimum: new(5, -2, 0), maximum: new(11, 4, 7));
		Point point = new(pointX, pointY, pointZ);

		// Act
		var result = boundingBox.Contains(point);

		// Assert
		Assert.False(result);
	}

	[Theory]
	[MemberData(nameof(GetPointsInsideBox))]
	public void Contains_ShouldBeTrue_WhenBoxInsideBox(Point minPoint, Point maxPoint)
	{
		// Arrange
		BoundingBox boundingBox = new(minimum: new(5, -2, 0), maximum: new(11, 4, 7));
		BoundingBox boundingBox2 = new(minimum: minPoint, maximum: maxPoint);

		// Act
		var result = boundingBox.Contains(boundingBox2);

		// Assert
		Assert.True(result);
	}
	public static IEnumerable<object[]> GetPointsInsideBox()
	{
        yield return new object[] { new Point(5, -2, 0), new Point(11, 4, 7) };
        yield return new object[] { new Point(6, -1, 1), new Point(10, 3, 6) };
    }

	[Theory]
	[MemberData(nameof(GetPointsOutsideBox))]
	public void Contains_ShouldBeFalse_WhenBoxOutsideBox(Point minPoint, Point maxPoint)
	{
		// Arrange
		BoundingBox boundingBox = new(minimum: new(5, -2, 0), maximum: new(11, 4, 7));
		BoundingBox boundingBox2 = new(minimum: minPoint, maximum: maxPoint);

		// Act
		var result = boundingBox.Contains(boundingBox2);

		// Assert
		Assert.False(result);
	}
	public static IEnumerable<object[]> GetPointsOutsideBox()
	{
		yield return new object[] { new Point(4, -3, -1), new Point(10, 3, 6) };
        yield return new object[] { new Point(6, -1, 1), new Point(12, 5, 8) };
	}

	[Fact]
	public void Transform_ShouldAdjustMinMax_WhenTransformGiven()
	{
        // Arrange
        BoundingBox boundingBox = new(minimum: new(-1, -1, -1), maximum: new(1, 1, 1));
        Matrix transform = Matrix.RotationX(Math.PI / 4) * Matrix.RotationY(Math.PI / 4);
		Point expectedMinimum = new(-1.4142, -1.7071, -1.7071);
		Point expectedMaximum = new(1.4142, 1.7071, 1.7071);

        // Act
        var result = boundingBox.Transform(transform);

        // Assert
        Assert.Equal(expectedMinimum, result.Minimum);
        Assert.Equal(expectedMaximum, result.Maximum);
    }
}
