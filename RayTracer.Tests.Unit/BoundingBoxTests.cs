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
		const bool expected = true;

		// Act
		var actual = boundingBox.Contains(point);

		// Assert
		Assert.Equal(expected, actual);
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
		BoundingBox boundingBox = new();
		Point point = new(pointX, pointY, pointZ);
		const bool expected = false;

		// Act
		var actual = boundingBox.Contains(point);

		// Assert
		Assert.Equal(expected, actual);
	}
}
