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
}
